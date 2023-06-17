using Acr.UserDialogs.Extended;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TornfyApp.API;
using TornfyApp.Model;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TornfyApp.ViewModel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Estatisticas : ContentPage
    {

        public Jogador info_jogador;
        public Stats performance;
        public List<Stats> table_performance_etapas;

        public int id_jogador;
        public int id_master;

        public Estatisticas(int id_jogador, int id_master)
        {
            InitializeComponent();

            this.id_jogador= id_jogador;
            this.id_master= id_master;

            stk_stats.IsVisible = true;
            stk_semregistro.IsVisible = false;

            try
            {
                if (TestarInternet()) CarregarDadosJogador(id_jogador);
                if (TestarInternet()) CarregarPerformance(id_jogador, id_master, DateTime.Now.Year);
                if (TestarInternet()) CarregarTableEtapasPerformance(id_jogador, id_master, DateTime.Now.Year);
            }
            catch (Exception)
            {
                stk_stats.IsVisible = false;
                stk_semregistro.IsVisible = true;
            }

            Title = string.Format("Estatíticas {0}", DateTime.Now.Year);

            var anosList = new List<int>();
            for (int i = 2015; i < DateTime.Today.Year + 1; i++)
            {
                anosList.Add(i);
            }

            pcAnos.Title = DateTime.Today.Year.ToString();
            pcAnos.ItemsSource = anosList;

        }

        public static bool TestarInternet()
        {
            bool coneccao = true;
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                coneccao = false;
                var pop2 = new MensagemComConfirmacao("Conecção Caiu!",
                    "Algo deu errado! Não há conecção de interenet.", "#E10555", "Fechar Mensagem?", false, "", false, "", 0);
                _ = App.Current.MainPage.Navigation.PushPopupAsync(pop2, true);
            }
            return coneccao;
        }

        public async void CarregarPerformance(int id_jogador, int id_master, int ano)
        {
            UserDialogs.Instance.ShowLoading(title: "Buscando Lista");
            performance = await API_Service.ObterPerformance(id_jogador, id_master, ano);

            if (performance != null)
            {
                int totalPartidas = performance.SomaVitorias + performance.SomaDerrotas;
                int total_Games = performance.SomaGamesGanhos + performance.SomaGamesPerdidos;
                int total_Sets = performance.SomaSetGanhos + performance.SomaSetPerdidos;

                txt_tpg.Text = performance.rating_elo.ToString();

                try
                {
                    if (performance.SomaGamesGanhos == 0)
                    {
                        somagames_ganhos.Text = "Não computados";
                        prg_somagames_ganhos.Progress = 0.5;
                        lbl_somagames_ganhos.Text = string.Format("Performance em Games ({0})", "0%");
                    }
                    else
                    {
                        somagames_ganhos.Text = "Games: " + performance.SomaGamesGanhos.ToString();
                        int per_games = (performance.SomaGamesGanhos * 100) / total_Games;
                        prg_somagames_ganhos.Progress = per_games;
                        lbl_somagames_ganhos.Text = string.Format("Performance em Games ({0}%)", per_games);
                    }
                }
                catch (Exception)
                {
                    somagames_ganhos.Text = "Não computados";
                    prg_somagames_ganhos.Progress = 0.5;
                    lbl_somagames_ganhos.Text = string.Format("Performance em Games ({0})", "0%");
                }

                try
                {
                    if (performance.SomaSetGanhos == 0)
                    {
                        somaset_ganhos.Text = "Não computados";
                        prg_somasets_ganhos.Progress = 0.5;
                        lbl_somasets_ganhos.Text = string.Format("Performance em Sets ({0})", "0%");
                    }
                    else
                    {
                        somaset_ganhos.Text = "Sets: " + performance.SomaSetGanhos.ToString();
                        int per_sets = (performance.SomaSetGanhos * 100) / total_Sets;
                        prg_somasets_ganhos.Progress = per_sets;
                        lbl_somasets_ganhos.Text = string.Format("Performance em Sets ({0}%)", per_sets);
                    }
                }
                catch (Exception)
                {
                    somaset_ganhos.Text = "Não computados";
                    prg_somasets_ganhos.Progress = 0.5;
                    lbl_somasets_ganhos.Text = string.Format("Performance em Sets ({0})", "0%");
                }

                try
                {
                    if (performance.SomaVitorias == 0)
                    {
                        soma_vitorias.Text = "Não computados";
                        prg_vitorias.Progress = 0.5;
                        lbl_vitorias.Text = string.Format("Performance em Partidas Vencidas ({0})", "0%");
                    }
                    else
                    {
                        soma_vitorias.Text = "Vitórias: " + performance.SomaVitorias.ToString();
                        int per_vit = (performance.SomaVitorias * 100) / totalPartidas;
                        prg_vitorias.Progress = per_vit;
                        lbl_vitorias.Text = string.Format("Performance em Vitotias ({0}%)", per_vit);
                    }
                }
                catch (Exception)
                {
                    soma_vitorias.Text = "Não computados";
                    prg_vitorias.Progress = 0.5;
                    lbl_vitorias.Text = string.Format("Performance em Partidas Vencidas ({0})", "0%");
                }

                try
                {
                    if (performance.SomaDerrotas == 0)
                    {
                        soma_derrotas.Text = "Não computados";
                        prg_derrotas.Progress = 0.5;
                        lbl_derrotas.Text = string.Format("Performance em Partidas Vencidas ({0})", "0%");
                    }
                    else
                    {
                        soma_derrotas.Text = "Derrotas: " + performance.SomaDerrotas.ToString();
                        int per_der = (performance.SomaDerrotas * 100) / totalPartidas;
                        prg_derrotas.Progress = per_der;
                        lbl_derrotas.Text = string.Format("Performance em Derrotas ({0}%)", per_der);
                    }
                }
                catch (Exception)
                {
                    soma_derrotas.Text = "Não computados";
                    prg_derrotas.Progress = 0.5;
                    lbl_derrotas.Text = string.Format("Performance em Partidas Vencidas ({0})", "0%");
                }

                Stats item1 = new Stats
                {
                    item = performance.SomaVitorias.ToString(),
                    item_index = 1,
                    texto_botao = "Vitorias"
                };
                Stats item2 = new Stats
                {
                    item = performance.SomaDerrotas.ToString(),
                    item_index = 2,
                    texto_botao = "Derrotas"
                };
                Stats item3 = new Stats
                {
                    item = performance.SomaSetGanhos.ToString(),
                    item_index = 3,
                    texto_botao = "Sets Vencidos"
                };
                Stats item4 = new Stats
                {
                    item = performance.SomaSetPerdidos.ToString(),
                    item_index = 4,
                    texto_botao = "Sets Perdidos"
                };
                Stats item5 = new Stats
                {
                    item = performance.SomaGamesGanhos.ToString(),
                    item_index = 5,
                    texto_botao = "Games Vencidos"
                };
                Stats item6 = new Stats
                {
                    item = performance.TotalPartidas.ToString(),
                    item_index = 5,
                    texto_botao = "Total Partidas"
                };
                Stats item7 = new Stats
                {
                    item = performance.Total_Games.ToString(),
                    item_index = 5,
                    texto_botao = "Total Games"
                };
                Stats item8 = new Stats
                {
                    item = performance.Total_Sets.ToString(),
                    item_index = 5,
                    texto_botao = "Total Sets"
                };

                //txt_n_jogos.Text = string.Format("Jogos Realizados {0}", performance.TotalPartidas);

                List<Stats> lsita_performance = new List<Stats>();

                lsita_performance.Add(item1);
                lsita_performance.Add(item2);
                lsita_performance.Add(item3);
                lsita_performance.Add(item4);
                lsita_performance.Add(item5);
                lsita_performance.Add(item6);
                lsita_performance.Add(item7);
                lsita_performance.Add(item8);

                estatisticas.ItemsSource = lsita_performance;
            }
            else
            {
                stk_stats.IsVisible = false;
                stk_semregistro.IsVisible = true;
            }

            UserDialogs.Instance.HideLoading();
        }

        public async void CarregarTableEtapasPerformance(int id_jogador, int id_master, int ano)
        {
            UserDialogs.Instance.ShowLoading(title: "Buscando Lista");
            table_performance_etapas = await API_Service.ObterTableEtapasPerformance(id_jogador, id_master, ano);

            if (table_performance_etapas != null)
            {
                etapas.ItemsSource = table_performance_etapas.ToList();
            }
            else
            {
                stk_stats.IsVisible = false;
                stk_semregistro.IsVisible = true;
            }

            UserDialogs.Instance.HideLoading();
        }


        public async void CarregarDadosJogador(int id_jogador)
        {
            UserDialogs.Instance.ShowLoading(title: "Buscando Lista");

            info_jogador = await API_Service.ObterDadosJogador(id_jogador);

            
            //txt_nome.Text = info_jogador.Nome_Jogador;

            //string url_teste = info_jogador.Path;

            //if (!string.IsNullOrEmpty(url_teste))
            //{
            //    HttpWebResponse response = null;
            //    var request = (HttpWebRequest)WebRequest.Create(url_teste);
            //    request.Method = "HEAD";
            //    try { response = (HttpWebResponse)request.GetResponse(); image_jogador.Source = url_teste; }
            //    catch (WebException ex) { image_jogador.Source = "user_round.png"; }
            //    finally { if (response != null) { response.Close(); } }
            //}
            //else { image_jogador.Source = "user_round.png"; }

            UserDialogs.Instance.HideLoading();

        }

        private void etapas_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private async void etapas_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            Stats etapa = ((ListView)sender).SelectedItem as Stats;
            if (etapa == null)
            {
                return;
            }
            else
            {
                await Navigation.PushAsync(new DetalheEtapaPerformance(etapa));
            }

        }

        private void estatisticas_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private void estatisticas_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private void pcAnos_SelectedIndexChanged(object sender, EventArgs e)
        {
            stk_stats.IsVisible = true;
            stk_semregistro.IsVisible = false;

            int ano = int.Parse(pcAnos.SelectedItem.ToString());
            Title = string.Format("Estatíticas {0}", ano);
            try
            {
                if (TestarInternet()) CarregarDadosJogador(id_jogador);
                if (TestarInternet()) CarregarPerformance(id_jogador, id_master, ano);
                if (TestarInternet()) CarregarTableEtapasPerformance(id_jogador, id_master, ano);
            }
            catch (Exception)
            {
                stk_stats.IsVisible = false;
                stk_semregistro.IsVisible = true;
            }
        }

        private async void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            string url = "https://tornfy.com/RatingEloTornfy_WebView";
            await Navigation.PushAsync(new WebViewGlobal(url, "Rating Elo"));
        }
    }
}