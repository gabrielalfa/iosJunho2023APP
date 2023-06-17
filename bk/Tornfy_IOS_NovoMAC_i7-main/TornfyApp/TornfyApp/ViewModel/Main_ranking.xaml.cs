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
    public partial class Main_ranking : ContentPage
    {

        int id_jogador = 0;
        public Config_Home config = new Config_Home();
        List<Temporada> lista_circuitos;

        public Main_ranking(Config_Home config, int Id_jogador)
        {

            config.master = config.master;
            config.path = config.path;
            config.circuito = config.circuito;
            config.color = config.color;
            config.obs = config.obs;
            config.info = config.info;
            id_jogador = Id_jogador;

            InitializeComponent();
            CarregarCircuitos(1);
        }


        public async void CarregarCircuitos(int id_master)
        {
            UserDialogs.Instance.ShowLoading(title: "Buscando Lista");
            lista_circuitos = await API_Service.ObterCircuitos_Menu_all();
            UserDialogs.Instance.HideLoading();

            listagem_torneios.ItemsSource = lista_circuitos.ToList();

        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {

        }

        private async void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading(title: "Buscando Etapas");
            await Navigation.PushAsync(new Etapas_Recentes(config, id_jogador, 200, null, null, null));
            UserDialogs.Instance.HideLoading();
        }

        private void listagem_torneios_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private async void listagem_torneios_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Temporada ranking = ((ListView)sender).SelectedItem as Temporada;
            if (ranking == null)
            {
                return;
            }

            if (ranking.id_tipo_etapa == 2)
            {
                ranking.id_tipo_etapa = 3;
            }

            await Navigation.PushAsync(new Tipo_Ranking(ranking.grupo_nome, ranking.Nome_Ranking, ranking.id_temporarda, ranking.id_ranking, ranking.id_master, ranking.id_tipo_etapa));

        }

        private void txt_busca_SearchButtonPressed(object sender, EventArgs e)
        {

        }
    }
}