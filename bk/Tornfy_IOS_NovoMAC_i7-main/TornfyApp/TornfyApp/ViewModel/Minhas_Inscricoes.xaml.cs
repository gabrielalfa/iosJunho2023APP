using Acr.UserDialogs.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TornfyApp.API;
using TornfyApp.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TornfyApp.ViewModel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Minhas_Inscricoes : ContentPage
    {
        public Config_Home config;
        public List<Inscricao> Lista_incricoes;
        int id_jogador = 0;
        int id_master = 0;

        public Minhas_Inscricoes(Config_Home _config, int Id_jogador)
        {
            id_jogador = Id_jogador;
            id_master = _config.master;
            config = _config;

            InitializeComponent();
            CarregarInscricoes(id_jogador, id_master);

            etapas.RefreshCommand = new Command(() =>
            {
                CarregarInscricoes(id_jogador, id_master);
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() => etapas.IsRefreshing = false);
            });


        }



        protected override void OnAppearing()
        {
            base.OnAppearing();
            CarregarInscricoes(id_jogador, id_master);
        }


        public async void CarregarInscricoes(int id_jogador, int id_master)
        {
            UserDialogs.Instance.ShowLoading(title: "Buscando Lista");

            Lista_incricoes = await API_Service.ObterInscricoes_Jogador(id_jogador, id_master);

            if (Lista_incricoes.Count == 0)
            {
                etapas.ItemsSource = Lista_incricoes.ToList();
                txt_subtitle.Text = "Jogador ainda não tem incrições";
                await DisplayAlert("Aviso", "Jogador ainda não se increveu em uma etapa!", "OK");
                UserDialogs.Instance.HideLoading();
            }
            else
            {
                txt_subtitle.Text = "Lista de inscrições por jogador";
                etapas.ItemsSource = Lista_incricoes.ToList();
                UserDialogs.Instance.HideLoading();
            }

        }

        private void etapas_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private void etapas_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Inscricao inscricao = ((ListView)sender).SelectedItem as Inscricao;
            if (inscricao == null)
            {
                return;
            }

            Navigation.PushAsync(new Fechamento_Pagamento(config, id_jogador, id_master, inscricao.id_etapa, inscricao.id_tipo));

        }


    }
}