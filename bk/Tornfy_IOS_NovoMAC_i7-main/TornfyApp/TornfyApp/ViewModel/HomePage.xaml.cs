using Acr.UserDialogs.Extended;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TornfyApp.API;
using TornfyApp.Model;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;
using Xamarin.Forms.Xaml;

namespace TornfyApp.ViewModel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public Config_Home config;
        public List<Etapa> Lista_torneios_proximos;
        public List<Ranking> Lista_Etapas_Pontuadas;

        public List<Chaves> Lista_jogos_recentes;
        public string color = "#3897f0";
        public string path = "";
        public string circuito = "";
        public int master = 0;
        public int id_jogador;
        public Jogador info_jogador;
        List<int> lista_esportes = new List<int>();

        public HomePage(Config_Home _config, int _id_jogador)
        {
            InitializeComponent();
            CarregarConfig();


            config = _config;
            id_jogador = _id_jogador;
            carroucel_days.ItemsSource = GetBudgets_ranking();
            //if (TestarInternet()) CarregarTorneios(); else Navigation.PushAsync(new NoInternet());
            carroucel_horarios.RefreshCommand = new Command(() =>
            {
                if (TestarInternet()) CarregarTorneios(); else Navigation.PushAsync(new NoInternet());
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() => carroucel_horarios.IsRefreshing = false);
            });

            if (_config.nome_jogador == null)
            {
                CarregarDadosJogadorAsync(id_jogador);
            }
            else
            {
                string[] nomes = _config.nome_jogador.Split(' ');
                string nome = nomes[0];

            }
        }



        public static bool TestarInternet()
        {
            bool coneccao = true;
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                coneccao = false;
            }
            return coneccao;
        }

        private async Task CarregarConfig()
        {
            string[] esportes = { "tenis_ck", "beach_ck", "padel_ck", "squash_ck" };

            for (int i = 0; i < esportes.Length; i++)
            {
                string chave = esportes[i];
                string valor = await Xamarin.Essentials.SecureStorage.GetAsync(chave);

                if (string.IsNullOrEmpty(valor))
                {
                    valor = "yes";
                    await Xamarin.Essentials.SecureStorage.SetAsync(chave, valor);
                }
                else
                {
                    if (valor == "yes")
                    {
                        lista_esportes.Add(i + 1);
                    }
                }


            }

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            lista_esportes.Clear();
            CarregarConfig();
            if (TestarInternet()) CarregarTorneios(); else Navigation.PushAsync(new NoInternet());
        }

        public List<ObservableGroupCollection<string, Etapa>> DadosAgrupadosJogos { get; set; }

        public async void CarregarTorneios()
        {
            UserDialogs.Instance.ShowLoading(title: "Buscando Lista");
            try
            {

                List<Etapa> Lista_torneios = await API_Service.ObterEtapas_Home(lista_esportes);


                //carroucel_horarios.ItemsSource = Lista_torneios.ToList();
                DadosAgrupadosJogos = Lista_torneios
                                        .GroupBy(p => p.nome_mes_inicio_grupo)
                                        .Select(p => new ObservableGroupCollection<string, Etapa>(p)).ToList();


                carroucel_horarios.ItemsSource = DadosAgrupadosJogos;

            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Erro", "Ocorreu um erro ao buscar a lista de torneios. Por favor, verifique sua conexão com a internet e tente novamente.", "OK");
                await Navigation.PushAsync(new NoInternet());
            }

            UserDialogs.Instance.HideLoading();

        }

        private async void carroucel_days_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TestarInternet())
            {
                if (e.CurrentSelection.Count() != 0)
                {
                    Budget_Ranking item = e.CurrentSelection[0] as Budget_Ranking;
                    if (item != null) { ((CollectionView)sender).SelectedItem = null; }

                    switch (item.menu)
                    {
                        case 1:
                            //Em implementação
                            UserDialogs.Instance.ShowLoading(title: "Buscando Estatísticas");
                            await Navigation.PushAsync(new Estatisticas(id_jogador, 1));
                            UserDialogs.Instance.HideLoading();
                            break;
                        case 2:
                            UserDialogs.Instance.ShowLoading(title: "Buscando Horarários");
                            await Navigation.PushAsync(new Complexos(config, id_jogador));
                            UserDialogs.Instance.HideLoading();
                            break;
                        case 3:
                            UserDialogs.Instance.ShowLoading(title: "Buscando Jogos");
                            await Navigation.PushAsync(new Meu_Historico(config, id_jogador));
                            UserDialogs.Instance.HideLoading();
                            break;
                        case 4:
                            UserDialogs.Instance.ShowLoading(title: "Buscando Salvos");
                            await Navigation.PushAsync(new Favoritos(config, id_jogador));
                            UserDialogs.Instance.HideLoading();
                            break;
                        case 5:
                            UserDialogs.Instance.ShowLoading(title: "Buscando Dados");
                            await Navigation.PushAsync(new Dados_Jogador(config, id_jogador, 1));
                            UserDialogs.Instance.HideLoading();
                            break;
                        case 6:
                            UserDialogs.Instance.ShowLoading(title: "Buscando Inscrições");
                            await Navigation.PushAsync(new Minhas_Inscricoes(config, id_jogador));
                            UserDialogs.Instance.HideLoading();
                            break;
                        case 7:
                            UserDialogs.Instance.ShowLoading(title: "Buscando Meus Rankings");
                            await Navigation.PushAsync(new Etapas_Pontuadas(config, id_jogador));
                            UserDialogs.Instance.HideLoading();
                            break;
                        case 8:
                            UserDialogs.Instance.ShowLoading(title: "Buscando Meus Créditos");
                            await Navigation.PushAsync(new Creditos(id_jogador, 1));
                            UserDialogs.Instance.HideLoading();
                            break;
                        case 9:
                            //await Navigation.PushAsync(new CentralAjuda());
                            var uri = string.Format("https://tornfy.com/FAQ");
                            await OpenBrowser(uri);
                            break;
                        case 12:
                            await Navigation.PushAsync(new MasterPage(config, id_jogador, 4));
                            break;

                        case 16:
                            await Navigation.PushAsync(new Planos(config, id_jogador));
                            break;


                        case 14:
                            await Navigation.PushAsync(new AgendaAula(config, id_jogador));
                            break;


                        default:
                            break;
                    }

                }
            }
        }

        private async Task OpenBrowser(string uri)
        {
            try
            {
                await Browser.OpenAsync(uri, new BrowserLaunchOptions
                {
                    LaunchMode = BrowserLaunchMode.SystemPreferred,
                    TitleMode = BrowserTitleMode.Show,
                    PreferredToolbarColor = Color.AliceBlue,
                    PreferredControlColor = Color.Violet
                });
            }
            catch (Exception ex)
            {
                // An unexpected error occured. No browser may be installed on the device.
            }
        }

        private void carroucel_horarios_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private async void carroucel_horarios_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Etapa etapa = ((ListView)sender).SelectedItem as Etapa;
            if (etapa == null)
            {
                return;
            }

            await Navigation.PushAsync(new TB_TorneiosPage(etapa.Nome_Etapa, etapa.id, id_jogador, etapa.id_tipo, etapa.etapa_bool,
                etapa.publico_programacao, etapa.anexo, etapa.nome_anexo, etapa.nome_principal));

            

        }

        private ObservableCollection<Budget_Ranking> budgets_ranking;
        public ObservableCollection<Budget_Ranking> Budgets_ranking
        {
            get { return budgets_ranking; }
            set
            {
                budgets_ranking = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Budget_Ranking> GetBudgets_ranking()
        {
            return new ObservableCollection<Budget_Ranking>
            {
                // new Budget_Ranking { Name = "Feed",  Image = "feed1", menu = 12 },
                new Budget_Ranking { Name = "Pagamentos",  Image = "qrcode", menu = 6 },
                new Budget_Ranking { Name = "Stats",  Image = "stats", menu = 1 },
                new Budget_Ranking { Name = "Locações",  Image = "menu_calendario", menu = 2 },
                  new Budget_Ranking { Name = "Aulas",  Image = "menu_padel", menu = 14 },

                new Budget_Ranking { Name = "Jogos",  Image = "game", menu = 3 },
                new Budget_Ranking { Name = "Salvos",  Image = "favorito_preto", menu = 4 },
                new Budget_Ranking { Name = "Dados",  Image = "menu_user", menu = 5 },
                new Budget_Ranking { Name = "Meu Ranking",  Image = "trofeu_home", menu = 7 },
                new Budget_Ranking { Name = "Créditos",  Image = "menu_info", menu = 8 },
                 new Budget_Ranking { Name = "Planos",  Image = "ic_cart_black_24dp", menu = 16 },

                new Budget_Ranking { Name = "Ajuda",  Image = "menu_faq", menu = 9 },



            };
        }


        public async void CarregarDadosJogadorAsync(int id_jogador)
        {
            info_jogador = await API_Service.ObterDadosJogador(id_jogador);
            string[] nomes = info_jogador.Nome_Jogador.Split(' ');
            //string nome = nomes[0];
            //wolcome_name.Text = "olá, " + nome.ToString();
            if (!info_jogador.Atualizacao_2016)
            {
                var pop = new MessageBox("Atualização de clubes!", string.Format("Atualização de clubes {0} pendente! Acesse seus dados para atualizar", DateTime.Now.Year));
                _ = App.Current.MainPage.Navigation.PushPopupAsync(pop, true);


            }
        }


        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        private void etapas_rankeadas_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Dados_Jogador(config, id_jogador, 1));
        }

        private void carousel_torneios_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            Etapa etapa = ((ListView)sender).SelectedItem as Etapa;
            if (etapa == null)
            {
                return;
            }

            string textoalert = string.Format("ID: {0} - Etapa: {1}", etapa.id, etapa.Nome_Etapa);
            DisplayAlert("Etapa Selecionada", textoalert, "OK");

        }


        private ObservableCollection<Budget> budgets;
        public ObservableCollection<Budget> Budgets
        {
            get { return budgets; }
            set
            {
                budgets = value;
                OnPropertyChanged();
            }
        }

        private float amount;
        public float SelectedAmount
        {
            get { return amount; }
            set
            {
                amount = value;
                OnPropertyChanged();
            }
        }



        private ObservableCollection<Budget> GetBudgets()
        {

            //1   Tênis
            //2   Beach
            //3   Volei
            //4   FutVolei
            //5   Futsal

            return new ObservableCollection<Budget>
            {
                new Budget { Name = "Tênis", Amount = 650, Color = Color.DeepSkyBlue, Image = "tennis.png", tipo_etapa = 1 },
                new Budget { Name = "Beach Tênis", Amount = 1350, Color = Color.SlateBlue, Image = "beach_tennis.png", tipo_etapa= 2 },
                new Budget { Name = "Volei", Amount = 170, Color = Color.Purple, Image = "volei.png" , tipo_etapa= 3},
                new Budget { Name = "FutVolei", Amount = 750, Color = Color.LightSeaGreen, Image = "futvolei.png", tipo_etapa = 4 },
            };
        }

        public int tipo_etapa_sel;

        private async void btnEtapas_Clicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading(title: "Buscando Etapas");
            await Navigation.PushAsync(new Etapas_Recentes(config, id_jogador, tipo_etapa_sel, null, null, null));
            UserDialogs.Instance.HideLoading();
        }

        private async void ItemTapped_ranking(object sender, EventArgs e)
        {
            var grid = sender as Grid;
            var selectedItem = grid.BindingContext as Budget_Ranking;
            int tipo_ranking_sel = selectedItem.tipo_etapa;

            UserDialogs.Instance.ShowLoading(title: "Buscando Rankings");
            await Navigation.PushAsync(new Etapas_Pontuadas(config, id_jogador));
            UserDialogs.Instance.HideLoading();

        }

        private void lista_jogos_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private async void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading(title: "Buscando Etapas");
            await Navigation.PushAsync(new Etapas_Recentes(config, id_jogador, tipo_etapa_sel, null, null, null));
            UserDialogs.Instance.HideLoading();
        }
    }

    public class Budget
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public float Amount { get; set; }
        public Color Color { get; set; }

        public int tipo_etapa { get; set; }

    }

    public class Budget_Ranking
    {
        public string Name { get; set; }
        public int menu { get; set; }
        public string Image { get; set; }
        public float Amount { get; set; }
        public Color Color { get; set; }

        public string text_string { get; set; }

        public int tipo_etapa { get; set; }

    }

}



