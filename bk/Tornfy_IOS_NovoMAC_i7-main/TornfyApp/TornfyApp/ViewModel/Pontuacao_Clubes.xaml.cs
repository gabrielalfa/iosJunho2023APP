using API_Inteleco.Models;
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
    public partial class Pontuacao_Clubes : ContentPage
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

        public Pontuacao_Clubes(int _id_temporada, int _id_ranking, int _tipo_cat, int _id_master, string _nome_ranking, int _tipo_simples, int _id_grupo)
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

            Title = "Pontuação";
            txt_subtitulo.Text = _nome_ranking;

            CarregarEtapasPonrtuadas();

        }

        public async void CarregarEtapasPonrtuadas()
        {

            _lista_Pontos = await API_Service.ObterEtapasPontosClubes(id_temporada, id_ranking, id_master);
            lista_jogos.ItemsSource = _lista_Pontos.ToList();


        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {

        }

        private void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {

        }

        private void lista_jogos_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }

        private void lista_jogos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
    }
}