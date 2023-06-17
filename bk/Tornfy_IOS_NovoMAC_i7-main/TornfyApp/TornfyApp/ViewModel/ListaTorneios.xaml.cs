using Acr.UserDialogs.Extended;
using Rg.Plugins.Popup.Extensions;
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
    public partial class ListaTorneios : ContentPage
    {
        public List<Etapa> Lista_torneios;
        public static string Info;
        public int id_jogador;
        public Config_Home config;
        public int _id;
        public List<circuitos_favoritos> circ_favs;
        public int id_grupo;
        public string circuito;

        public ListaTorneios(Config_Home _config, int Id_jogador)
        {
            InitializeComponent();


            Info = config.info;
            circuito = config.circuito;
            id_jogador = Id_jogador;

            ToolbarItems.Remove(no_fav);
            ToolbarItems.Remove(fav);

            id_grupo = int.Parse(_config.info.Split('-')[2].ToString());

            if (TestarInternet()) CarregarTorneios(_config.info, "vazio");

            listagem_torneios.RefreshCommand = new Command(() =>
            {
                if (TestarInternet()) CarregarTorneios(_config.info, "vazio");
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() => listagem_torneios.IsRefreshing = false);
            });


        }

        public static bool TestarInternet()
        {
            bool coneccao = true;
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                coneccao = false;
                var pop2 = new MensagemComConfirmacao("Conecção Caiu!",
                    "Algo deu errado! Não há conecção de interenet.", "#E10555", "Fechar Mensagem?", false, "", false, "", 0);
                _ = App.Current.MainPage.Navigation.PushPopupAsync(pop2, true);
            }
            return coneccao;
        }

        public async void CarregarTorneios(string info, string busca)
        {
            UserDialogs.Instance.ShowLoading(title: "Buscando Lista");
            Lista_torneios = await API_Service.ObterEtapas(info, busca);
            UserDialogs.Instance.HideLoading();

            listagem_torneios.ItemsSource = Lista_torneios.ToList();

            listagem_torneios.IsRefreshing = false;
        }

        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    CarregarTorneios(Info, "vazio");
        //}

        private void listagem_torneios_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private async void Listagem_torneios_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            Etapa etapa = ((ListView)sender).SelectedItem as Etapa;
            if (etapa == null)
            {
                return;
            }

            config.nome_etapa = etapa.Nome_Etapa;

            await Navigation.PushAsync(new TB_TorneiosPage(etapa.Nome_Etapa, etapa.id, id_jogador, etapa.id_tipo, etapa.etapa_bool, etapa.publico_programacao, etapa.anexo, etapa.nome_anexo, etapa.nome_principal));

        }


        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {

        }

        private void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {

        }

        private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            CarregarTorneios(Info, txt_busca.Text);
        }




        private async void fav_Clicked(object sender, EventArgs e)
        {
            //await App.Database_circuitos.DeleteEtapa_Favotita(new circuitos_favoritos
            //{
            //    id = _id
            //});
            ToolbarItems.Add(no_fav);
            ToolbarItems.Remove(fav);
            var pop = new MessageBox("Circuito Desfavoritado!", "Circuito foi retirado da sua lista de Circuitos favoritos!");
            await Application.Current.MainPage.Navigation.PushPopupAsync(pop, true).ConfigureAwait(false);
        }

        private async void no_fav_Clicked(object sender, EventArgs e)
        {
            bool save = true;
            if (id_grupo != 0)
            {
                if (circ_favs.Count == 0)
                {
                    save = true;
                }

                foreach (var item in circ_favs)
                {
                    if (item.id_grupo == id_grupo)
                    {
                        save = false;
                    }
                }
                if (save)
                {
                    //await App.Database_circuitos.SaveEtapa_Favotita(new circuitos_favoritos
                    //{
                    //    id_grupo = id_grupo,
                    //    nome_circuito = circuito,
                    //    info = Info
                    //});

                    ToolbarItems.Add(fav);
                    ToolbarItems.Remove(no_fav);
                    var pop = new MessageBox("Circuito Favoritado!", "Agora este circuito está disponível em suas etapas favoritas para rápido acesso!");
                    await Application.Current.MainPage.Navigation.PushPopupAsync(pop, true).ConfigureAwait(false);
                }

            }

        }
    }
}