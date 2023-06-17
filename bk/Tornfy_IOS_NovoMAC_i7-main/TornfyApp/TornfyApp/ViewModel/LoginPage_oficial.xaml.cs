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
    public partial class LoginPage_oficial : ContentPage
    {

        public List<Temporada> lista_circuitos;
        public string color = "#3897f0";
        public string path = "";
        public string circuito = "";
        public int master = 1;
        public string obs = "";
        public string info = "";
        public static Jogador info_jogador;

        public static string email;
        public static string senha;

        public Login info_login;

        public int id_jogador;
        public Task<int> id_jogador_login;

        public List<Login> circ_favs;

        public Config_Home config = new Config_Home();

        public LoginPage_oficial(bool delete, int id_jogador_pos, string email_pos, string pass_pos)
        {
            //logout
            if (delete)
            {
                SecureStorage.Remove("id_jogador");
                SecureStorage.Remove("password");
                SecureStorage.Remove("email");
                SecureStorage.Remove("path");
            }

            InitializeComponent();
            config.master = 1;
            CarregarSecureStorage();
            if (id_jogador_pos != 0) { CarregarDadosJogadorAsync(id_jogador_pos, email_pos, pass_pos); }

        }

        public async void CarregarDadosJogadorAsync(int id_jogador, string email_pos, string pass_pos)
        {

            if (TestarInternet())
            {
                Jogador info_jogador = await API_Service.ObterDadosLogin_teste(email_pos, pass_pos, 1);

                id_jogador = info_jogador.id_jogador;
                config.nome_jogador = info_jogador.Nome_Jogador;
                config.email_jogador = info_jogador.Email;

                add_login(id_jogador.ToString(), email_pos, pass_pos, info_jogador.Nome_Jogador, info_jogador.Path, info_jogador.telefone);

                await Navigation.PushAsync(new MasterPage(config, id_jogador, 1), true);
                Navigation.RemovePage(this);

            }
            else
            {
                _ = Navigation.PushAsync(new NoInternet());
            }

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

        public async void CarregarSecureStorage()
        {

            string id_jogador_str = await Xamarin.Essentials.SecureStorage.GetAsync("id_jogador");
            if (!string.IsNullOrEmpty(id_jogador_str)) { id_jogador = int.Parse(id_jogador_str); } else { id_jogador = 0; }
            string pass = await Xamarin.Essentials.SecureStorage.GetAsync("password");
            string _email = await Xamarin.Essentials.SecureStorage.GetAsync("email");

            string nome_jogador = await Xamarin.Essentials.SecureStorage.GetAsync("nome_jogador");
            string path = await Xamarin.Essentials.SecureStorage.GetAsync("path");

            if (pass != null) { txtSenha.Text = pass.ToString(); }
            if (_email != null) { txtEmail.Text = _email.ToString(); }

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtSenha.Text))
            {
                var pop = new MessageBox("Informe os Dados!", "Informe todos os dados solicitados no formulario.");
                _ = App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
            }
            else
            {

                email = txtEmail.Text;
                senha = txtSenha.Text;

                if (TestarInternet())
                {
                    Jogador info_jogador = await API_Service.ObterDadosLogin_teste(txtEmail.Text, txtSenha.Text, 1);


                    if (info_jogador != null)
                    {
                        if (info_jogador.Email.ToLower().Trim() == txtEmail.Text.ToLower().Trim() && info_jogador.password.ToLower().Trim() == txtSenha.Text.ToLower().Trim())
                        {

                            id_jogador = info_jogador.id_jogador;
                            config.nome_jogador = info_jogador.Nome_Jogador;
                            config.email_jogador = info_jogador.Email;

                            add_login(info_jogador.id_jogador.ToString(), txtEmail.Text.Trim(), txtSenha.Text.Trim(), info_jogador.Nome_Jogador, info_jogador.Path, info_jogador.telefone);

                            await Navigation.PushAsync(new MasterPage(config, id_jogador, 1), true);
                            Navigation.RemovePage(this);
                        }
                        else
                        {
                            var pop = new MessageBox("Dados Incorretos!", "Informe novamente seus dados ou confira se seu cadastro é valido!");
                            _ = App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
                        }
                    }
                    else
                    {
                        var pop = new MessageBox("Dados Incorretos!", "Informe novamente seus dados ou confira se seu cadastro é valido!");
                        _ = App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
                    }



                }
                else await Navigation.PushAsync(new NoInternet());
            }

        }

        async Task Task_Void_Async()
        {
            await Task.Delay(5000);
            Console.WriteLine("5 segundos de atraso");
        }

        public async Task CarregarLogin()
        {
            info_jogador = await API_Service.ObterDadosLogin(email, senha, 1);
            await Task_Void_Async();
        }



        private void Button_Clicked_1(object sender, EventArgs e)
        {
            var viewPage = new ConfirmarCPF();
            var navigation = Application.Current.MainPage as NavigationPage;
            navigation.PushAsync(viewPage, true);
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (id_jogador != 0)
            {
                config.master = 1;
                Navigation.PushAsync(new MasterPage(config, id_jogador, 1), true);
            }

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }


        private async void add_login(string id_jogador, string email, string pass, string nome_jogador, string path, string telefone)
        {
            await Xamarin.Essentials.SecureStorage.SetAsync("id_jogador", id_jogador);
            await Xamarin.Essentials.SecureStorage.SetAsync("password", pass);
            await Xamarin.Essentials.SecureStorage.SetAsync("email", email);
            await Xamarin.Essentials.SecureStorage.SetAsync("nome_jogador", nome_jogador);
            await Xamarin.Essentials.SecureStorage.SetAsync("path", path);
            await Xamarin.Essentials.SecureStorage.SetAsync("telefone", telefone);

            //var pop = new MessageBox("Login realizado!", "Login realizado com sucesso!");
            //await Application.Current.MainPage.Navigation.PushPopupAsync(pop, false).ConfigureAwait(true);

        }


    }
}