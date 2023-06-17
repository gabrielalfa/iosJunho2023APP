using API_Inteleco.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TornfyApp.API;
using TornfyApp.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TornfyApp.ViewModel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [Obsolete]
    public partial class MasterPage : MasterDetailPage
    {
        private ObservableCollection<MasterPageItem> _menuLista;
        public Config_Home config;

        public string color = "#3897f0";
        public string path = "";
        public string circuito = "";
        public int master = 0;
        public int id_jogador;
        public object[] args { get; set; }
        // Define uma variável para controlar o loop
        private bool isLooping = true;

        public MasterPage(Config_Home _config, int Id_jogador, int page)
        {

            config = _config;
            id_jogador = Id_jogador;


            InitializeComponent();

            StartButton_Clicked(40000);

            _menuLista = ItemService.GetMenuItens(id_jogador, config);
            navigationDrawerList.ItemsSource = _menuLista;

            CarregarSecureStorage();

            try
            {
                switch (page)
                {
                    case 1:
                        Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(HomePage), args = new object[] { _config, id_jogador }));
                        break;
                    case 2:
                        Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(ListaTorneios), args = new object[] { _config, id_jogador }));
                        break;
                    case 3:
                        Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(Fechamento_Pagamento), args = new object[] { _config, id_jogador, _config.master, _config.id_etapa, _config.id_tipo }));
                        break;
                    case 13:
                        Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(AgendaAula), args = new object[] { id_jogador }));
                        break;
                    case 4:
                        Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(TimeLine), args = new object[] { _config, id_jogador }));
                        break;
                    default:
                        Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(HomePage), args = new object[] { _config, id_jogador }));
                        break;
                }
            }
            catch (Exception)   
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(ErrorPage)));
            }



        }

        // Método que executa o loop
        private async void StartLoop(int time)
        {
            while (isLooping)
            {
                // Espera 5 segundos
                await Task.Delay(time);
                CarregarDadosJogadorAsync(id_jogador);
            }
        }

        // Método que inicia o loop
        private void StartButton_Clicked(int time)
        {
            isLooping = true;
            Device.StartTimer(TimeSpan.FromSeconds(5), () =>
            {
                StartLoop(time);
                return isLooping;
            });
        }

        // Método que para o loop
        private void StopButton_Clicked(object sender, EventArgs e)
        {
            isLooping = false;
        }



        public async void CarregarSecureStorage()
        {
            email_jogador.Text = await Xamarin.Essentials.SecureStorage.GetAsync("email");
            nome_jogador.Text = await Xamarin.Essentials.SecureStorage.GetAsync("nome_jogador");

            string url_teste = "https://tornfy.com/" + await Xamarin.Essentials.SecureStorage.GetAsync("path");

            string toCheck = "https://tornfy.com/https://tornfy.com/";
            bool contains = url_teste.Contains(toCheck);
            if (contains)
            {
                string toRemove = "https://tornfy.com/https://tornfy.com/";
                url_teste = url_teste.Replace(toRemove, "");
                url_teste = "https://tornfy.com/" + url_teste;
            }


            if (!string.IsNullOrEmpty(url_teste))
            {
                HttpWebResponse response = null;
                var request = (HttpWebRequest)WebRequest.Create(url_teste);
                request.Method = "HEAD";
                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                    img_jogador.Source = url_teste;
                }
                catch (WebException ex)
                {
                    img_jogador.Source = "user_round.png";
                }
                finally { if (response != null) { response.Close(); } }
            }
            else { img_jogador.Source = "user_round.png"; }
        }


        private void navigationDrawerList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {


                string d_inicio = "";
                string d_fim = "";
                string pub = "";

                var item = (MasterPageItem)e.SelectedItem;
                if (item == null)
                    return;
                //obtem o tipo de objeto 
                Type pagina = item.TargetType;

                switch (pagina.Name)
                {
                    case "Planos":
                        Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(Planos), args = new object[] { config, id_jogador }));
                        break;
                    case "AgendaAula":
                        Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(AgendaAula), args = new object[] { config, id_jogador }));
                        break;
                    case "CentralAjuda":
                        Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(CentralAjuda)));
                        break;
                    case "Locacoes":
                        Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(Complexos), args = new object[] { config, id_jogador }));
                        break;
                      case "ListaTorneiosModalidade":
                        int tipo_etapa = int.Parse(item.args[1].ToString());
                        Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(ListaTorneiosModalidade), args = new object[] { id_jogador, tipo_etapa, d_inicio, d_fim, pub }));
                        break;
                    case "Circuitos_Modalidade":
                        int tipo_etapa_sel = int.Parse(item.args[1].ToString());
                        Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(Circuitos_Modalidade), args = new object[] { id_jogador, tipo_etapa_sel }));
                        break;
                    case "HomePage":
                        Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(HomePage), args = new object[] { config, id_jogador }));
                        break;
                    case "Minhas_Inscricoes":
                        Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(Minhas_Inscricoes), args = new object[] { config, id_jogador }));
                        break;
                    case "Dados_Jogador":
                        Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(Dados_Jogador), args = new object[] { config, id_jogador }));
                        break;
                    case "Etapas_Pontuadas":
                        Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(Etapas_Pontuadas), args = new object[] { config, id_jogador }));
                        break;
                    case "MainPage":
                        Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(MainPage), args = new object[] { config, id_jogador }));
                        break;
                    case "Historico_Jogadores":
                        Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(Historico_Jogadores), args = new object[] { config, id_jogador }));
                        break;
                    case "Clubes":
                        Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(Clubes), args = new object[] { config.master }));
                        break;
                    case "Regulamentos":
                        Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(Regulamentos)));
                        break;
                    case "Main_ranking":
                        Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(Main_ranking), args = new object[] { config, id_jogador }));
                        break;
                    case "Login_2":
                        bool delete = true;
                        int id_jogador_pos = 0;
                        Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(LoginPage_oficial), args = new object[] { delete, id_jogador_pos, "", "" }));
                        break;
                    case "ConfigPage":
                        //Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(ConfigPage)));

                        Navigation.PushAsync(new ConfigPage());


                        break;
                    default:
                        Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(HomePage), args = new object[] { config, id_jogador }));
                        break;
                }
                IsPresented = false;

            }
            catch (Exception)
            {


            }
                
        }

        public async void CarregarDadosJogadorAsync(int id_jogador)
        {
            Jogador info_jogador = await API_Service.ObterDadosJogador(id_jogador);
            string toCheck = "https://tornfy.com";
            bool contains = info_jogador.Path.Contains(toCheck);
            string url;
            if (contains)
            {
                //string toRemove = "https://tornfy.com";
                //string result = info_jogador.Path.Replace(toRemove, "");
                url = info_jogador.Path;
            }
            else
            {   
                url = "https://tornfy.com" + info_jogador.Path;
            }

            await Xamarin.Essentials.SecureStorage.SetAsync("path", url);
            img_jogador.Source = url;
        }

        private void navigationDrawerList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;

        }

        protected override void OnAppearing()
        {

            base.OnAppearing();
            CarregarDadosJogadorAsync(id_jogador);
            CarregarSecureStorage();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            CarregarDadosJogadorAsync(id_jogador);
            CarregarSecureStorage();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            StartButton_Clicked(20000);
            await Navigation.PushAsync(new Dados_Jogador(config, id_jogador, 1));
        }

        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
           
            await Navigation.PushAsync(new Dados_Jogador(config, id_jogador, 1));
        }


    }
}