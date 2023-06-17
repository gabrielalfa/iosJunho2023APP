using Acr.UserDialogs.Extended;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class Circuito : ContentPage
    {
        public List<Etapa> Lista_torneios;

        public int id_jogador;
        public Config_Home config;
        public int id_grupo;
        public string d_inicio;
        public string d_fim;
        public string pub;
        public Temporada ranking;
        public int id_del;
        public int tipo_etapa;

        public string path;
        public string Nome_Ranking;

        public Circuito(int id_jogador, int id_grupo, string d_inicio, string d_fim, string pub, string path, string Nome_Ranking, Temporada ranking, bool favorito, int id_del, int tipo_etapa)
        {
            InitializeComponent();

            this.id_grupo = id_grupo;
            this.id_jogador = id_jogador;
            this.d_inicio = d_inicio;
            this.d_fim = d_fim;
            this.pub = pub;
            this.ranking = ranking;
            this.id_del = id_del;
            this.tipo_etapa = tipo_etapa;

            this.path = path;
            this.Nome_Ranking = Nome_Ranking;

            img_path.Source = path;
            Title = Nome_Ranking;

            if (TestarInternet()) CarregarTorneiosRecentes();

            ToolbarItems.Remove(no_fav);
            ToolbarItems.Remove(fav);

            if (favorito)
            {
                ToolbarItems.Remove(no_fav);
                ToolbarItems.Add(fav);
            }
            else
            {
                ToolbarItems.Add(no_fav);
                ToolbarItems.Remove(fav);
            }

            listagem_torneios.RefreshCommand = new Command(() =>
            {
                if (TestarInternet()) CarregarTorneiosRecentes();
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() => listagem_torneios.IsRefreshing = false);
            });

            var anosList = new List<int>();
            for (int i = 2015; i < DateTime.Today.Year + 1; i++)
            {
                anosList.Add(i);
            }

            pcAnos.Title = DateTime.Today.Year.ToString();
            pcAnos.ItemsSource = anosList;


        }

        public async void CarregarTorneiosRecentes()
        {
            int qtd = 30;

            //string busca = txt_busca.Text;
            //if (busca == null) busca = "vazio";

            UserDialogs.Instance.ShowLoading(title: "Buscando Etapas");
            Lista_torneios = await API_Service.ObterTorneiosCircuito(qtd, "vazio", id_grupo, d_inicio, d_fim, pub);
            listagem_torneios.ItemsSource = Lista_torneios.ToList();
            UserDialogs.Instance.HideLoading();

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


        private void listagem_torneios_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private async void listagem_torneios_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Etapa etapa = ((ListView)sender).SelectedItem as Etapa;
            if (etapa == null)
            {
                return;
            }

            if (etapa != null)
            {

                await Navigation.PushAsync(new TB_TorneiosPage(etapa.Nome_Etapa, etapa.id, id_jogador, etapa.id_tipo, etapa.etapa_bool, etapa.publico_programacao, etapa.anexo, etapa.nome_anexo, etapa.nome_principal));
            }


        }

        private void txt_busca_SearchButtonPressed(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading(title: "Buscando Etapas");
            if (TestarInternet()) CarregarTorneiosRecentes();
            UserDialogs.Instance.HideLoading();
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            //definir na api para temporada atual caso zero
            int id_temporarda = 0;

            await Navigation.PushAsync(new Tipo_Ranking(ranking.grupo_nome, ranking.Nome_Ranking, id_temporarda, ranking.id_ranking, ranking.id_master, ranking.tipo_etapa));
        }

        private async void fav_Clicked(object sender, EventArgs e)
        {
            if (id_grupo != 0)
            {
                var dados = new Salvos
                {
                    id = id_grupo,
                    circuito_bool = false,
                    id_jogador = id_jogador,
                };

                int result = await API_Service.EditarCircuitoFavorito(dados);
                if (result == 200)
                {
                    ToolbarItems.Add(no_fav);
                    ToolbarItems.Remove(fav);
                    var pop = new MessageBox("Circuito Desfavoritado!", "Circuito foi retirada da sua lista de etapas favoritos!");
                    await Application.Current.MainPage.Navigation.PushPopupAsync(pop, true).ConfigureAwait(false);
                }
                else
                {
                    var pop = new MessageBox("Falha na requisição!", "Houve uma falha no momento na solicitação, aguarde uns instantes e tente novamente!");
                    await Application.Current.MainPage.Navigation.PushPopupAsync(pop, true).ConfigureAwait(false);
                }

            }




        }

        private async void no_fav_Clicked(object sender, EventArgs e)
        {
            if (id_grupo != 0)
            {
                var dados = new Salvos
                {
                    id = id_grupo,
                    circuito_bool = true,
                    id_jogador = id_jogador,
                };

                int result = await API_Service.EditarCircuitoFavorito(dados);
                if (result == 200)
                {
                    ToolbarItems.Add(fav);
                    ToolbarItems.Remove(no_fav);
                    var pop = new MessageBox("Circuito Favoritado!", "Agora este circuito está disponível em seus circuitos favotiros para rápido acesso!");
                    await Application.Current.MainPage.Navigation.PushPopupAsync(pop, true).ConfigureAwait(false);
                }
                else
                {
                    var pop = new MessageBox("Falha na requisição!", "Houve uma falha no momento na solicitação, aguarde uns instantes e tente novamente!");
                    await Application.Current.MainPage.Navigation.PushPopupAsync(pop, true).ConfigureAwait(false);
                }

            }
        }

        private async void pcAnos_SelectedIndexChanged(object sender, EventArgs e)
        {
            int qtd = 30;

            string busca = "vazio";

            int ano = int.Parse(pcAnos.SelectedItem.ToString());

            d_inicio = string.Format("01/01/{0}", ano);
            d_fim = string.Format("30/12/{0}", ano);

            UserDialogs.Instance.ShowLoading(title: "Buscando Etapas");
            Lista_torneios = await API_Service.ObterTorneiosRecentes_Geral(qtd, busca, tipo_etapa, d_inicio, d_fim, pub);
            listagem_torneios.ItemsSource = Lista_torneios.ToList();
            UserDialogs.Instance.HideLoading();
        }
    }
}