using Acr.UserDialogs.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TornfyApp.API;
using TornfyApp.Model;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TornfyApp.ViewModel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Complexos : ContentPage
    {
        public int id_jogador;
        public string telefone; public string email; public string nome_jogador;

        List<Quadra> lista_locais;
        List<Quadra> lista_locais_outros;
        public Config_Home config;

        public Complexos(Config_Home config, int id_jogador)
        {
            InitializeComponent();
            this.id_jogador = id_jogador;
            this.config = config;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            CarregarComplexos();
            CarregarSecureStorage();

        }



        public async void CarregarComplexos()
        {
            try
            {
                UserDialogs.Instance.ShowLoading(title: "Buscando Locais");
                lista_locais = await API_Service.ObterLocaisLocacao(id_jogador);
                UserDialogs.Instance.HideLoading();

                listagem_torneios.ItemsSource = lista_locais.ToList();

            }
            catch (Exception)
            {
                await Navigation.PushAsync(new ErrorPage());
            }

        }

        public async void CarregarSecureStorage()
        {
            string nome_jogador = await Xamarin.Essentials.SecureStorage.GetAsync("nome_jogador");
            string telefone = await Xamarin.Essentials.SecureStorage.GetAsync("telefone");
            string email = await Xamarin.Essentials.SecureStorage.GetAsync("email");

            if (string.IsNullOrEmpty(telefone) || string.IsNullOrEmpty(email))
            {
                CarregarDadosJogador(id_jogador);
            }

            this.telefone = telefone;
            this.email = email;
            this.nome_jogador = nome_jogador;


        }

        public async void CarregarDadosJogador(int id_jogador)
        {
            var info_jogador = await API_Service.ObterDadosJogador(id_jogador);
            await Xamarin.Essentials.SecureStorage.SetAsync("email", info_jogador.Email);
            await Xamarin.Essentials.SecureStorage.SetAsync("telefone", info_jogador.telefone);
        }

        private void listagem_torneios_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private async void listagem_torneios_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Quadra local = ((ListView)sender).SelectedItem as Quadra;
            if (local == null)
            {
                return;
            }
            UserDialogs.Instance.ShowLoading(title: "Buscando Quadras e Horários");
            await Navigation.PushAsync(new QuadrasComplexo(config, local.id_master, id_jogador, local.nome_locacoes, local.id_proprietario));
            UserDialogs.Instance.HideLoading();

        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading(title: "Buscando Etapas");
            await Navigation.PushAsync(new Etapas_Recentes(config, id_jogador, 200, null, null, null));
            UserDialogs.Instance.HideLoading();
        }

        private void wolcome_name_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Dados_Jogador(config, id_jogador, 1));
        }

        private async void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading(title: "Buscando Etapas");
            await Navigation.PushAsync(new Etapas_Recentes(config, id_jogador, 0, null, null, null));
            UserDialogs.Instance.HideLoading();
        }


        private async void btn_naoValores_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Outros_Complexos(id_jogador, telefone, email, nome_jogador));
        }
    }
}