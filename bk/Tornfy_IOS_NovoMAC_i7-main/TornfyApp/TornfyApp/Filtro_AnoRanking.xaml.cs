using Acr.UserDialogs.Extended;
using API_Inteleco.Models;
using ICT.ViewModel;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TornfyApp.API;
using TornfyApp.Model;
using TornfyApp.ViewModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TornfyApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Filtro_AnoRanking : PopupPage
    {
        public List<Temporada> Lista_Anos;

        public List<Categoria> Lista_Categorias;
        public List<Ranking> _lista_Pontos;

        public int id_temporada;
        public int id_ranking;
        public int tipo_cat;
        public int id_master;
        public string nome_ranking;
        public int tipo_simples;
        public int id_grupo;
        public int ano;

        public Filtro_AnoRanking(int _id_temporada, int ano, int _id_ranking, int _tipo_cat, int _id_master, string _nome_ranking, int _tipo_simples, int _id_grupo)
        {
            InitializeComponent();

            id_temporada = _id_temporada;
            id_ranking = _id_ranking;
            id_master = _id_master;
            tipo_cat = _tipo_cat;
            nome_ranking = _nome_ranking;
            tipo_simples = _tipo_simples;
            //determina se beach ou tenis
            id_grupo = _id_grupo;
            this.ano = ano;

            if (TestarInternet()) CarregarTorneiosRecentes();

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

        public async void CarregarTorneiosRecentes()
        {
            Lista_Anos = await API_Service.ObterAnosTemporada(1);
            pcCategoria.ItemsSource = Lista_Anos.ToList();
        }

        private void pcCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            Temporada ranking = ((Picker)sender).SelectedItem as Temporada;
            if (ranking == null)
            {
                return;
            }

            id_temporada = ranking.id;
            ano = int.Parse(ranking.Ano);

        }

        private async void btn_close_Clicked(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new Pontuacao_Ranking(id_temporada, ano, id_ranking, tipo_cat, id_master, nome_ranking, tipo_cat, id_grupo));

            //_ = App.Current.MainPage.Navigation.PopPopupAsync(true);

        }
    }
}