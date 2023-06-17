using Rg.Plugins.Popup.Extensions;
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
    public partial class Etapas_Pontuadas : ContentPage
    {
        public List<Ranking> Lista_Etapas_Pontuadas;
        public int id_master = 0;
        public int id_jogador = 0;
        public string nome_jogador = "";

        public Etapas_Pontuadas(Config_Home config, int Id_jogador)
        {
            InitializeComponent();

            id_master = config.master;
            id_jogador = Id_jogador;
            //nome_jogador = config.nome_jogador;

            txt_nome.Text = "Etapas Pontuadas";
            txt_top.Text = nome_jogador.ToString();

            CarregarEtapasPonrtuadas(DateTime.Today.Year);

            var anosList = new List<int>();
            for (int i = 2010; i < DateTime.Today.Year + 1; i++)
            {
                anosList.Add(i);
            }

            pcAnos.Title = DateTime.Today.Year.ToString();
            pcAnos.ItemsSource = anosList;

        }

        public async void CarregarEtapasPonrtuadas(int ano)
        {
            Lista_Etapas_Pontuadas = await API_Service.ObterEtapasPontuadas_Jogador(id_jogador, ano);

            if (Lista_Etapas_Pontuadas.Count == 0)
            {
                await DisplayAlert("Aviso", "Jogador ainda não pontuou em etapas!", "OK");
            }
            else
            {
                etapas_rankeadas.ItemsSource = Lista_Etapas_Pontuadas.ToList();
            }



        }

        private void etapas_rankeadas_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private void etapas_rankeadas_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Ranking ranking = ((ListView)sender).SelectedItem as Ranking;
            if (ranking == null)
            {
                return;
            }

            Navigation.PushAsync(new PotuacaoParcial(ranking.id_jogador, ranking.ano, ranking.id_categoria, ranking.Nome_Categoria, ranking.Nome_Jogador, ranking.img_path));

        }

        private void btn_revisao_Clicked(object sender, EventArgs e)
        {
            var pop = new MessageBox("Revisão Solicitada!", "Aguarde alguns instantes e atualize a página de pontuação!");
            App.Current.MainPage.Navigation.PushPopupAsync(pop, true);

            etapas_rankeadas.RefreshCommand = new Command(() =>
            {
                CarregarEtapasPonrtuadas(DateTime.Today.Year);
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() => etapas_rankeadas.IsRefreshing = false);
            });
        }

        private void pcAnos_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarEtapasPonrtuadas(int.Parse(pcAnos.SelectedItem.ToString()));
        }
    }
}