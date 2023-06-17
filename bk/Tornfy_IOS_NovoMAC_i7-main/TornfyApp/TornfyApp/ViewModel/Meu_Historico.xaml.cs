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
    public partial class Meu_Historico : ContentPage
    {
        public List<ObservableGroupCollection<string, Chaves>> DadosAgrupadosJogos { get; set; }

        public List<Chaves> Lista_jogos_recentes;
        public Config_Home config = new Config_Home();
        public int id_jogador;

        public Meu_Historico(Config_Home config, int Id_jogador)
        {
            id_jogador = Id_jogador;
            InitializeComponent();
            CarregarSecureStorage();
            CarregarHistorico_Jogos(DateTime.Today.Year);

            var anosList = new List<int>();
            for (int i = 2015; i < DateTime.Today.Year + 1; i++)
            {
                anosList.Add(i);
            }

            pcAnos.Title = DateTime.Today.Year.ToString();
            pcAnos.ItemsSource = anosList;

        }

        public async void CarregarSecureStorage()
        {
            string id_jogador_str = await Xamarin.Essentials.SecureStorage.GetAsync("id_jogador");
            if (!string.IsNullOrEmpty(id_jogador_str))
            {
                int id_logador = int.Parse(id_jogador_str);
                int id_visitante = id_jogador;

                if (id_logador == id_visitante)
                {
                    string nome_jogador = await Xamarin.Essentials.SecureStorage.GetAsync("nome_jogador");
                   
                    Title = "Meus Jogos";
                }
                else
                {
                    string result = await CarregarDadosJogador(id_visitante);
                    
                    Title = result;
                }

            }
        }

        public async Task<string> CarregarDadosJogador(int id_visitante)
        {
            Jogador info_jogador = await API_Service.ObterDadosJogador(id_visitante);
            return info_jogador.Nome_Jogador;
        }



        public async void CarregarHistorico_Jogos(int ano)
        {
            int qtd = 0;
            Lista_jogos_recentes = await API_Service.ObterHistorico_Jogos(qtd, id_jogador, ano);


            if (Lista_jogos_recentes.Count == 0)
            {
                no_data.IsVisible = true;
                lista_jogos.IsVisible = false;
            }
            else
            {
                no_data.IsVisible = false;
                lista_jogos.IsVisible = true;
            }

            DadosAgrupadosJogos = Lista_jogos_recentes
                                     .GroupBy(p => p.Nome_etapa)
                                     .Select(p => new ObservableGroupCollection<string, Chaves>(p)).ToList();


            lista_jogos.ItemsSource = DadosAgrupadosJogos;


        }

        private void pcCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ano = int.Parse(pcAnos.SelectedItem.ToString());
            CarregarHistorico_Jogos(ano);
        }

        private void lista_jogos_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private async void lista_jogos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Chaves jogo = ((ListView)sender).SelectedItem as Chaves;
            if (jogo == null)
            {
                return;
            }
            else
            {
                await Navigation.PushAsync(new Estatisticas(id_jogador, 1));
            }


        }
    }
}