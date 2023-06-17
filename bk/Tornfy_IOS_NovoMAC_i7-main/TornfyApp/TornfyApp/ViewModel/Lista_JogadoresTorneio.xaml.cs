using Acr.UserDialogs.Extended;
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
    public partial class Lista_JogadoresTorneio : ContentPage
    {


        public List<Jogador> Lista_jogadores_inscritos;
        public List<Categoria> Lista_Categorias;
        //public int id_etapa = 0;
        Etapa etp = new Etapa();


        public Lista_JogadoresTorneio(int id_etapa)
        {
            InitializeComponent();
            CarregarCategoriasEtapa(id_etapa);


        }

        public async void CarregarJogadoresEtapa(int id_cat, int id_etapa)
        {


            UserDialogs.Instance.ShowLoading(title: "Buscando Lista");

            Lista_jogadores_inscritos = await API_Service.ObterJogadoresEtapa(id_etapa, id_cat);

            if (Lista_jogadores_inscritos.Count == 0)
            {
                lista_jogadores.ItemsSource = Lista_jogadores_inscritos.ToList();
                txt_subtitle.Text = "Categoria ainda não tem inscritos";
                await DisplayAlert("Aviso", "Categoria ainda não tem inscritos", "OK");
                UserDialogs.Instance.HideLoading();
            }
            else
            {
                txt_subtitle.Text = "Lista de Jogadores por categoria";
                lista_jogadores.ItemsSource = Lista_jogadores_inscritos.ToList();
                UserDialogs.Instance.HideLoading();
            }

        }

        public List<ObservableGroupCollection<string, Categoria>> DadosAgrupados { get; set; }

        public async void CarregarCategoriasEtapa(int id_etapa)
        {
            Lista_Categorias = await API_Service.ObterCategoriaEtapa(id_etapa);

            pcCategoria.ItemsSource = Lista_Categorias.OrderBy(x => x.Categoria_Nome).ToList();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

        }

        private void lista_jogadores_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private void pcCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {

            Categoria categoria = ((Picker)sender).SelectedItem as Categoria;
            if (categoria == null)
            {
                return;
            }

            //DisplayAlert("Categoria Selecionada", categoria.id.ToString(), "OK");

            CarregarJogadoresEtapa(categoria.id, categoria.id_etapa);
        }
    }
}