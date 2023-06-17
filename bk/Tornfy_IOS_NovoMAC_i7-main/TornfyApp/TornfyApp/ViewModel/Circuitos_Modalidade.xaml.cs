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
    public partial class Circuitos_Modalidade : ContentPage
    {
        public List<Etapa> Lista_torneios;

        public int id_jogador;
        public Config_Home config;
        public int tipo_etapa;

        public List<Temporada> circuitos;

        public string d_inicio;
        public string d_fim;

        public string pub;

        public Circuitos_Modalidade(int id_jogador, int tipo_etapa)
        {
            InitializeComponent();

            this.tipo_etapa = tipo_etapa;
            this.id_jogador = id_jogador;

            switch (tipo_etapa)
            {
                case 1:
                    txt_busca.Placeholder = string.Format("Buscar Circuitos {0}...", "Tênis");
                    Title = string.Format("Circuitos de {0}", "Tênis");
                    break;
                case 2:
                    txt_busca.Placeholder = string.Format("Buscar Circuitos {0}...", "Beach");
                    Title = string.Format("Circuitos de {0}", "Beach");
                    break;
                case 3:
                    txt_busca.Placeholder = string.Format("Buscar Circuitos {0}...", "Volei");
                    Title = string.Format("Circuitos de {0}", "Volei");
                    break;
                case 4:
                    txt_busca.Placeholder = string.Format("Buscar Circuitos {0}...", "FutVolei");
                    Title = string.Format("Circuitos de {0}", "FutVolei");
                    break;
                case 5:
                    txt_busca.Placeholder = string.Format("Buscar Circuitos {0}...", "Futsal");
                    Title = string.Format("Circuitos de {0}", "Futsal");
                    break;
                case 6:
                    txt_busca.Placeholder = string.Format("Buscar Circuitos {0}...", "Futsal");
                    Title = string.Format("Circuitos de {0}", "Futsal");
                    break;
                case 7:
                    txt_busca.Placeholder = string.Format("Buscar Circuitos {0}...", "Squash");
                    Title = string.Format("Circuitos de {0}", "Squash");
                    break;
                default:
                    break;
            }

            if (TestarInternet()) CarregarCircuitos();

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

        protected override void OnAppearing()
        {
            if (TestarInternet()) CarregarCircuitos();
        }

        public async void CarregarCircuitos()
        {
            UserDialogs.Instance.ShowLoading(title: "Buscando Lista");
            circuitos = await API_Service.ObterCircuitos_Modalidade(tipo_etapa, id_jogador, txt_busca.Text);
            UserDialogs.Instance.HideLoading();

            listagem_circuitos.ItemsSource = circuitos.ToList();

        }

        private void listagem_circuitos_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private async void listagem_circuitos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Temporada ranking = ((ListView)sender).SelectedItem as Temporada;
            if (ranking == null)
            {
                return;
            }

            await Navigation.PushAsync(new Circuito(id_jogador, ranking.id_grupo, "", "", "", ranking.path, ranking.grupo_nome, ranking, ranking.circ_fav, ranking.id_del, tipo_etapa));

        }

        private void txt_busca_SearchButtonPressed(object sender, EventArgs e)
        {
            CarregarCircuitos();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            Temporada ranking = ((Button)sender).BindingContext as Temporada;
            if (ranking == null)
            {
                return;
            }

            await Navigation.PushAsync(new Circuito(id_jogador, ranking.id_grupo, "", "", "", ranking.path, ranking.grupo_nome, ranking, ranking.circ_fav, ranking.id_del, tipo_etapa));
        }

        private async void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading(title: "Buscando Etapas");
            await Navigation.PushAsync(new Etapas_Recentes(config, id_jogador, tipo_etapa, null, null, null));
            UserDialogs.Instance.HideLoading();
        }
    }
}