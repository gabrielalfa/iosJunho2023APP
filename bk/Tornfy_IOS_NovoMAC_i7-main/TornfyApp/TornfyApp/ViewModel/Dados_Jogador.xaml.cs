using Acr.UserDialogs.Extended;
using API_Inteleco.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class Dados_Jogador : ContentPage
    {
        private ObservableCollection<DadosJogador_item> _menuLista;

        public Jogador info_jogador;
        public string color = "#3897f0";
        public string path = "";
        public string circuito = "";
        public int id_master;
        public int id_jogador = 0;

        int id_clube_tenis = 0;
        int id_clube_Btenis = 0;


        string nome_clube = "";
        string nome_clube_beach = "";
        public Config_Home config;



        public Dados_Jogador(Config_Home _config, int id_jogador, int id_master)
        {
            InitializeComponent();

            config = _config;
            this.id_jogador = id_jogador;
            this.id_master = id_master;

            CarregarDadosJogador(id_jogador);
            CarregarSecureStorage();
        }


        public async void CarregarDadosJogador(int id_jogador)
        {
            UserDialogs.Instance.ShowLoading(title: "Buscando Lista");

            info_jogador = await API_Service.ObterDadosJogador(id_jogador);

            txt_email.Text = info_jogador.Email;
            txt_nome.Text = info_jogador.Nome_Jogador;

            nome_clube = info_jogador.String_Clube;
            nome_clube_beach = info_jogador.clube_beach_string;

            id_clube_tenis = info_jogador.id_Clube;
            id_clube_Btenis = int.Parse(info_jogador.clube_beach);

            _menuLista = ItemService.GetDadosJogador(info_jogador);
            dados_jogador.ItemsSource = _menuLista; 

            image_jogador.Source = info_jogador.Path;
            await Xamarin.Essentials.SecureStorage.SetAsync("path", info_jogador.Path);



            if (!info_jogador.Atualizacao_2016)
            {
                aviso_atualizacao.BackgroundColor = Color.FromHex("#EC407A");
                aviso_texto.TextColor = Color.FromHex("#fff");
                aviso_texto.Text = string.Format("Atualização de clubes {0} pendente!", DateTime.Now.Year);
            }
            else
            {
                aviso_atualizacao.BackgroundColor = Color.FromHex("#ffd900");
                aviso_texto.TextColor = Color.FromHex("#000");
                aviso_texto.Text = string.Format("Atualização de clubes {0} atualizada! ", DateTime.Now.Year);
            }

            UserDialogs.Instance.HideLoading();

        }

        public async void CarregarSecureStorage()
        {
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
                    image_jogador.Source = url_teste;
                }
                catch (WebException ex)
                {
                    image_jogador.Source = "user_round.png";
                }
                finally
                {
                    if (response != null) { response.Close(); }
                }
            }
            else { image_jogador.Source = "user_round.png"; }
        }


        private void dados_jogador_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CarregarDadosJogador(id_jogador);
            CarregarSecureStorage();

        }

        private void btnEditarDados(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Editar_DadosJogador(id_jogador, id_master));
        }

        private void ToolbarItem_Clicked1(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private async void dados_jogador_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (DadosJogador_item)e.SelectedItem;

            if (item == null)
            {
                return;
            }

            bool cl_edit = item.clube_edit;

            //direcionar para a plataforma de torneios com os dados de login
            if (!info_jogador.Atualizacao_2016)
            {
                var uri = string.Format("https://tornfy.com/Login/Login_Usuario_AppAtualizacao?Login_Usuario={0}&Password={1}&url_back={2}", info_jogador.Jogador_Email, info_jogador.password, "/DadosPessoais?tab=1");
                await OpenBrowser(uri);
            }
            else
            {
                if (cl_edit)
                {
                    if (item.tipo_clube == 10)
                    {
                        DisplayAlert("Edição não permitida", "Edição não permitida", "Fechar");
                    }
                    else
                    {
                        //clube beach
                        if (item.tipo_clube == 1)
                        {
                            //clube avulso
                            if (id_clube_Btenis != 10)
                            {
                                DisplayAlert("Edição não permitida", "Só é permitido a edição de clubes avulsos, segundo regulamento", "Fechar");
                            }
                            else
                            {
                                Navigation.PushAsync(new Editar_ClubeJogador(config, id_jogador, id_master, item.tipo_clube, nome_clube_beach));

                            }

                        }
                        //clube tenis
                        else if (item.tipo_clube == 2)
                        {
                            //clube avulso
                            if (id_clube_tenis != 10)
                            {
                                DisplayAlert("Edição não permitida", "Só é permitido a edição de clubes avulsos, segundo regulamento", "Fechar");
                            }
                            else
                            {
                                Navigation.PushAsync(new Editar_ClubeJogador(config, id_jogador, id_master, item.tipo_clube, nome_clube));
                            }

                        }

                    }
                }
                else
                {
                    Navigation.PushAsync(new Editar_DadosJogador(id_jogador, id_master));
                }
            }



        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (!info_jogador.Atualizacao_2016)
            {
                var uri = string.Format("https://tornfy.com/Login/Login_Usuario_AppAtualizacao?Login_Usuario={0}&Password={1}&url_back={2}", info_jogador.Jogador_Email, info_jogador.password, "/DadosPessoais?tab=1");
                await OpenBrowser(uri);

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

        private async void TapGestureRecognizer_Tapped_1(System.Object sender, System.EventArgs e)
        {
            //string url = string.Format("https://tornfy.com/Tornfy/MinhaFoto_WebView?id={0}", id_jogador);

            //await Navigation.PushAsync(new WebViewGlobal(url, "Editar Foto"));
        }
    }
}