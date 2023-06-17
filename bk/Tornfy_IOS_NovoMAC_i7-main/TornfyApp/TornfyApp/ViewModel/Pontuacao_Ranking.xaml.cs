using API_Inteleco.Models;
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
    public partial class Pontuacao_Ranking : ContentPage
    {

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


        public Pontuacao_Ranking(int _id_temporada, int ano, int _id_ranking, int _tipo_cat, int _id_master, string _nome_ranking, int _tipo_simples, int _id_grupo)
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
            if (ano == 0) ano = DateTime.Today.Year;
            Title = string.Format("Pontuação {0}", ano);
            txt_subtitulo.Text = _nome_ranking;

            if (TestarInternet()) CarregarCategoriasRanking();

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

        public async void CarregarCategoriasRanking()
        {
            Lista_Categorias = await API_Service.ObterEtapasCategoriasRanking(id_master, tipo_simples, id_grupo);
            pcCategorias.ItemsSource = Lista_Categorias.ToList();
        }

        public async void CarregarEtapasPonrtuadas(int id_categoria)
        {

            _lista_Pontos = await API_Service.ObterEtapasPontosCategoria(id_temporada, id_ranking, id_categoria);
            lista_jogos.ItemsSource = _lista_Pontos.ToList();

            if (_lista_Pontos != null)
            {
                //try
                //{
                //    var cat_grupo = _lista_Pontos.Select(x => x.grupo_categoria).FirstOrDefault();
                //    switch (cat_grupo)
                //    {
                //        case "Feminino":
                //            bola_rk.Source = "ball_tennis_pink.png";
                //            break;
                //        case "Masculino":
                //            bola_rk.Source = "ball_tennis.png";
                //            break;
                //        case "Infantil":
                //            bola_rk.Source = "ball_tennis_beach.png";
                //            break;
                //        case "Senior":
                //            bola_rk.Source = "ball_tennis_green.png";
                //            break;
                //        default:
                //            break;
                //    }

                //}
                //catch (Exception)
                //{ }
            }


        }



        private void pcCategorias_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            Categoria categoria = ((Picker)sender).SelectedItem as Categoria;
            if (categoria == null)
            {
                return;
            }

            CarregarEtapasPonrtuadas(categoria.id);
        }

        private void lista_jogos_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }

        private void lista_jogos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private async void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Filtro_AnoRanking(id_temporada, ano, id_ranking, tipo_cat, id_master, nome_ranking, tipo_cat, id_grupo));
        }
    }
}