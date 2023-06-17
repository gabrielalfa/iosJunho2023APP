using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TornfyApp.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TornfyApp.ViewModel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Tipo_Ranking : ContentPage
    {

        public ObservableCollection<List<Ranking>> ranking;

        public int id_temporada;
        public int id_ranking;
        public int id_master;
        public int id_grupo;

        public Tipo_Ranking(string grupo_nome, string Nome_Ranking, int _id_temporarda, int _id_ranking, int _id_master, int _id_grupo)
        {
            InitializeComponent();
            carrega_tipos();
            listagem_torneios.ItemsSource = carrega_tipos();

            id_ranking = _id_ranking;
            id_temporada = _id_temporarda;
            id_master = _id_master;

            titulo.Text = grupo_nome;
            sub_titulo.Text = Nome_Ranking;
            //determina se tenis ou beach   
            id_grupo = _id_grupo;

        }

        private ObservableCollection<Budget_Ranking> rankings;
        public ObservableCollection<Budget_Ranking> Rankings
        {
            get { return rankings; }
            set
            {
                rankings = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Budget_Ranking> carrega_tipos()
        {

            return new ObservableCollection<Budget_Ranking>
            {
                new Budget_Ranking { Nome_Ranking = "Ranking Jogador Simples", img_path ="ball_tennis.png", tipo_cat = "1", rk_clube= false},
                new Budget_Ranking {  Nome_Ranking = "Ranking Jogador Duplas", img_path ="ball_tennis_blue.png",tipo_cat = "0" , rk_clube= false},
                new Budget_Ranking {  Nome_Ranking = "Ranking Clubes Simples", img_path ="ball_tennis.png",tipo_cat = "1" , rk_clube= true},
                new Budget_Ranking {  Nome_Ranking = "Ranking Clubes Duplas", img_path = "ball_tennis_blue.png" ,tipo_cat = "0", rk_clube= true},
            };
        }

        private void listagem_torneios_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private async void listagem_torneios_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Budget_Ranking ranking = ((ListView)sender).SelectedItem as Budget_Ranking;
            if (ranking == null)
            {
                return;
            }


            if (ranking.rk_clube)
            {
                await Navigation.PushAsync(new Pontuacao_Clubes(id_temporada, id_ranking, int.Parse(ranking.tipo_cat), id_master, ranking.Nome_Ranking, int.Parse(ranking.tipo_cat), id_grupo));
            }
            else
            {
                await Navigation.PushAsync(new Pontuacao_Ranking(id_temporada, 0, id_ranking, int.Parse(ranking.tipo_cat), id_master, ranking.Nome_Ranking, int.Parse(ranking.tipo_cat), id_grupo));
            }


        }


        public class Budget_Ranking
        {
            public string Nome_Ranking { get; set; }
            public string img_path { get; set; }
            public float Amount { get; set; }
            public Color Color { get; set; }

            public string tipo_cat { get; set; }

            public int tipo_etapa { get; set; }

            public bool rk_clube { get; set; }

        }


    }
}