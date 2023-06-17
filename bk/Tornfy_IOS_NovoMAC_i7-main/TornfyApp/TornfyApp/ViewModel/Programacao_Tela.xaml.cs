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
    public partial class Programacao_Tela : ContentPage
    {

        public List<Programacao> Lista_Jogos;
        public string nome_etapa;
        public string nome_jogador;
        public int id_etapa;

        public Programacao_Tela(int _id_etapa, string _nome_etapa, string _nome_jogador)
        {
            InitializeComponent();

            nome_etapa = _nome_etapa;
            id_etapa = _id_etapa;
            nome_jogador = _nome_jogador;
            CarregarEtapasPonrtuadas(_nome_jogador);

            txt_busca.Text = _nome_jogador.ToString();

        }

        public async void CarregarEtapasPonrtuadas(string busca)
        {
            Lista_Jogos = await API_Service.ObterJogosTela(id_etapa, busca);
            listagem_torneios.ItemsSource = Lista_Jogos.ToList();

            if (Lista_Jogos.Count == 0)
            {
                no_data.IsVisible = true;
                listagem_torneios.IsVisible = false;
            }
            else
            {
                no_data.IsVisible = false;
                listagem_torneios.IsVisible = true;
            }

        }

        private async void txt_busca_SearchButtonPressed(object sender, EventArgs e)
        {
            Lista_Jogos = await API_Service.ObterJogosTela(id_etapa, txt_busca.Text);
            listagem_torneios.ItemsSource = Lista_Jogos.ToList();

            if (Lista_Jogos.Count == 0)
            {
                no_data.IsVisible = true;
                listagem_torneios.IsVisible = false;
            }
            else
            {
                no_data.IsVisible = false;
                listagem_torneios.IsVisible = true;
            }

        }

        private void listagem_torneios_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {

        }

        private void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {

        }




    }
}