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
    public partial class Historico_Jogadores : ContentPage
    {


        public List<Jogador> Lista_jogadores;
        public Config_Home config = new Config_Home();


        public Historico_Jogadores(Config_Home Config, int Id_etapa)
        {
            InitializeComponent();
            config = Config;
            CarregarJogadores();

        }

        public async void CarregarJogadores()
        {
            UserDialogs.Instance.ShowLoading(title: "Buscando Jogadores");
            Lista_jogadores = await API_Service.ObterCategoriaJogadores(config.master, txtSearch.Text);
            lista_categorias.ItemsSource = Lista_jogadores.Take(50);
            UserDialogs.Instance.HideLoading();
        }

        public async void CarregarJogadores_Busca()
        {
            Lista_jogadores = await API_Service.ObterCategoriaJogadores(config.master, txtSearch.Text);
            lista_categorias.ItemsSource = Lista_jogadores.Take(50);
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            CarregarJogadores_Busca();
        }

        private void lista_categorias_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private void txtSearch_SearchButtonPressed(object sender, EventArgs e)
        {
            CarregarJogadores_Busca();
        }

        private async void lista_categorias_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            Jogador jogador = ((ListView)sender).SelectedItem as Jogador;
            if (jogador == null)
            {
                return;
            }

            UserDialogs.Instance.ShowLoading(title: "Buscando Jogos");
            await Navigation.PushAsync(new Meu_Historico(config, jogador.id_Jogador));
            UserDialogs.Instance.HideLoading();
        }
    }
}