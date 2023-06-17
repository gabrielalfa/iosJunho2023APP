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
    public partial class Etapas_Recentes : ContentPage
    {

        public List<Etapa> Lista_torneios;
        public static string Info;
        public int id_jogador;
        public Config_Home config;
        public int tipo_etapa_sel;

        public string d_inicio;
        public string d_fim;
        public string pub;

        public Etapas_Recentes(Config_Home _config, int _id_jogador, int _tipo_etapa_sel, string d_inicio, string d_fim, string pub)
        {
            InitializeComponent();

            config = _config;
            if (config.circuito == null)
            {
                Title = "Etapas Recentes";
            }
            else
            {
                Title = _config.circuito;
            }

            Info = config.info;
            tipo_etapa_sel = _tipo_etapa_sel;
            id_jogador = _id_jogador;
            this.d_inicio = d_inicio;
            this.d_fim = d_fim;
            this.pub = pub;

            if (TestarInternet()) CarregarTorneiosRecentes();


            listagem_torneios.RefreshCommand = new Command(() =>
            {
                if (TestarInternet()) CarregarTorneiosRecentes();
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

        protected override bool OnBackButtonPressed()
        {
            Navigation.PopToRootAsync();
            return base.OnBackButtonPressed();
        }

        async void OnClosePageRequested()
        {
            var id = "d";
        }


        public async void CarregarTorneiosRecentes()
        {
            int qtd = 30;
            tipo_etapa_sel = 200;

            UserDialogs.Instance.ShowLoading(title: "Buscando Etapas");
            Lista_torneios = await API_Service.ObterTorneiosRecentes_Geral(qtd, "vazio", tipo_etapa_sel, d_inicio, d_fim, pub);
            listagem_torneios.ItemsSource = Lista_torneios.ToList();
            UserDialogs.Instance.HideLoading();

        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {

        }

        private async void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {
            var pop = new FiltroEtapas(config, id_jogador, tipo_etapa_sel, 1, 0, "", "");
            await Application.Current.MainPage.Navigation.PushPopupAsync(pop, false).ConfigureAwait(true);
        }


        private void listagem_torneios_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private async void listagem_torneios_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Etapa etapa = ((ListView)sender).SelectedItem as Etapa;
            if (etapa == null)
            {
                return;
            }

            config.nome_etapa = etapa.Nome_Etapa;

            await Navigation.PushAsync(new TB_TorneiosPage(etapa.Nome_Etapa, etapa.id, id_jogador, etapa.id_tipo, etapa.etapa_bool, etapa.publico_programacao, etapa.anexo, etapa.nome_anexo, etapa.nome_principal));

        }

        private async void txt_busca_SearchButtonPressed(object sender, EventArgs e)
        {
            int qtd = 20;

            string busca = txt_busca.Text;
            if (busca == null) busca = "vazio";

            UserDialogs.Instance.ShowLoading(title: "Buscando Etapas");
            Lista_torneios = await API_Service.ObterTorneiosRecentes_Geral(qtd, busca, tipo_etapa_sel, d_inicio, d_fim, pub);
            listagem_torneios.ItemsSource = Lista_torneios.ToList();
            UserDialogs.Instance.HideLoading();
        }

        //protected override bool OnBackButtonPressed()
        //{
        //     Navigation.PopToRootAsync();

        //    return base.OnBackButtonPressed();
        //}
    }
}