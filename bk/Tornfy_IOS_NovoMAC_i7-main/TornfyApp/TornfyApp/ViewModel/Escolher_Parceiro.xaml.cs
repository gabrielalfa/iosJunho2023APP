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
    public partial class Escolher_Parceiro : ContentPage
    {

        int id_etapa = 0;
        int id_jogador = 0;
        string nome_etapa = "";
        string nome_categoria = "";
        int id_categoria = 0;
        public List<Jogador> Lista_jogadores;
        public string nome_jogador;
        public int id_tipo;
        public int id_master;

        public List<ObservableGroupCollection<string, Jogador>> DadosAgrupados { get; set; }

        public Escolher_Parceiro(int id_master, int Id_etapa, int id_jogador_inscricao,
            string Nome_etp, int categoria, string categoria_nome, string _nome_jogador, int _id_tipo)
        {
            InitializeComponent();

            id_jogador = id_jogador_inscricao;
            id_etapa = Id_etapa;
            nome_etapa = Nome_etp;
            nome_categoria = categoria_nome;
            txt_nome_etapa.Text = nome_etapa.ToString();
            txt_nome_categoria.Text = categoria_nome;
            id_categoria = categoria;
            nome_jogador = _nome_jogador.ToString();
            id_tipo = _id_tipo;
            this.id_master = id_master;

        }

        public async void CarregarJogadores()
        {
            UserDialogs.Instance.ShowLoading(title: "Buscando Jogadores");
            Lista_jogadores = await API_Service.ObterCategoriaJogadores(id_master, txtSearch.Text);

            DadosAgrupados = Lista_jogadores.OrderBy(p => p.Nome_Jogador)
                             .GroupBy(p => p.Nome_Jogador[0].ToString())
                             .Select(p => new ObservableGroupCollection<string, Jogador>(p)).ToList();

            lista_categorias.ItemsSource = DadosAgrupados;
            UserDialogs.Instance.HideLoading();

        }

        private void lista_categorias_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private async void lista_categorias_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Jogador jogador = ((ListView)sender).SelectedItem as Jogador;
            if (jogador == null)
            {
                return;
            }

            UserDialogs.Instance.ShowLoading(title: "Buscando Jogadores");
            await Navigation.PushAsync(new Tamanho_Camiseta_Capitao(id_master, id_etapa, id_jogador,
                nome_etapa, id_categoria, nome_categoria, jogador.id_Jogador, jogador.Nome_Jogador, nome_jogador, id_tipo));
            UserDialogs.Instance.HideLoading();


        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            //UserDialogs.Instance.ShowLoading(title: "Buscando Jogadores");
            //CarregarJogadores();
            //UserDialogs.Instance.HideLoading();


        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

        }

        private async void txtSearch_SearchButtonPressed(object sender, EventArgs e)
        {

            UserDialogs.Instance.ShowLoading(title: "Buscando Lista");

            string contagem = txtSearch.Text;
            int cnt = contagem.Length;


            Lista_jogadores = await API_Service.ObterCategoriaJogadores(id_master, txtSearch.Text.Trim());

            DadosAgrupados = Lista_jogadores.OrderBy(p => p.Nome_Jogador)
                             .GroupBy(p => p.Nome_Jogador[0].ToString())
                             .Select(p => new ObservableGroupCollection<string, Jogador>(p)).ToList();

            lista_categorias.ItemsSource = DadosAgrupados;

            UserDialogs.Instance.HideLoading();

        }
    }
}