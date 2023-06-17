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
    public partial class Editar_ClubeJogador : ContentPage
    {
        int id_master = 0;
        int tipo_clube = 0;
        int id_jogador = 0;
        string nome_clube = "";
        public List<Clube> Lista_clubes;
        public Config_Home config;

        public List<ObservableGroupCollection<string, Clube>> DadosAgrupados { get; set; }

        public Editar_ClubeJogador(Config_Home Config, int Id_jogador, int master, int Tipo_clube, string Nome_clube)
        {
            id_master = master;
            tipo_clube = Tipo_clube;
            id_jogador = Id_jogador;
            nome_clube = Nome_clube;
            config = Config;

            InitializeComponent();

            txt_clube_Atual.Text = Nome_clube;


            switch (Tipo_clube)
            {
                case 1:
                    Title = "Editar Clube Beach";
                    break;
                case 2:
                    Title = "Editar Clube Tênis";
                    break;
                case 3:
                    Title = "Editar Clube Volei";
                    break;
                case 4:
                    Title = "Editar Clube Fut";
                    break;
                default:
                    break;
            }

            CarregarClubes();


        }

        public async void CarregarClubes()
        {

            UserDialogs.Instance.ShowLoading(title: "Buscando Clubes");

            Lista_clubes = await API_Service.ObterClubes(id_master, txtSearch.Text);

            DadosAgrupados = Lista_clubes.OrderBy(p => p.Nome_CLube)
                             .GroupBy(p => p.Nome_CLube[0].ToString())
                             .Select(p => new ObservableGroupCollection<string, Clube>(p)).ToList();

            lista_clubes.ItemsSource = DadosAgrupados;

            UserDialogs.Instance.HideLoading();



        }

        private void lista_clubes_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private void lista_clubes_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Clube clube = ((ListView)sender).SelectedItem as Clube;
            if (clube == null)
            {
                return;
            }



            Navigation.PushAsync(new Confirmar_Edicao_Clube(config, id_jogador, tipo_clube, clube.String_Clube, clube.id));

        }

        private async void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UserDialogs.Instance.ShowLoading(title: "Buscando Clubes");

            Lista_clubes = await API_Service.ObterClubes(id_master, txtSearch.Text);

            DadosAgrupados = Lista_clubes.OrderBy(p => p.Nome_CLube)
                             .GroupBy(p => p.Nome_CLube[0].ToString())
                             .Select(p => new ObservableGroupCollection<string, Clube>(p)).ToList();

            lista_clubes.ItemsSource = DadosAgrupados;

            UserDialogs.Instance.HideLoading();
        }
    }
}