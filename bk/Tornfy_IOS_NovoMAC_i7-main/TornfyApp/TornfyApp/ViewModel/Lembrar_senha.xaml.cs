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
    public partial class Lembrar_senha : ContentPage
    {
        public List<Temporada> lista_circuitos;
        public Lembrar_senha()
        {
            InitializeComponent();      
        }


        private async void Button_Clicked_2(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtCPF.Text))
            {
                var pop = new MessageBox("Informe os Dados!", "Informe o cpf solicitados no formulario.");
                _ = App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
            }
            else
            {
                if (TestarInternet())
                {
                    Formulario_Inscricao dados = new Formulario_Inscricao
                    {
                        cpf = txtCPF.Text
                    };

                    RetornoEnvio resultado = await API_Service.Lembrar_Senha_CPF(dados);

                    // 200 = cpf (encontrado) / email (encontratdo e viculado ao cpf) / email enviado 
                    // 500 = cpf inválido
                    // 400 = cpf inesistente
                    // 600 = email inválido
                    // 300 = email inisistente

                    int retorno = resultado.codigo_retorno;

                    switch (retorno)
                    {
                        case 200:
                            var viewPage = new LoginPage_oficial(false, 0, "", "");
                            var navigation = Application.Current.MainPage as NavigationPage;
                            var pop5 = new MessageBox("Lembrete de senha enviado!", string.Format("Um lembrete de senha foi enviado para o email: '{0}'", resultado.email_retorno));
                            _ = App.Current.MainPage.Navigation.PushPopupAsync(pop5, true);
                            await navigation.PushAsync(viewPage, true);
                            break;
                        case 500:
                            var pop1 = new MessageBox("CPF inválido!", "Informe um cpf verdadeiro!");
                            _ = App.Current.MainPage.Navigation.PushPopupAsync(pop1, true);
                            break;
                        case 400:
                            var pop2 = new MessageBox("CPF inesistente!", "Informe um CPF cadastrado em nossa base ou cadastre-se!");
                            _ = App.Current.MainPage.Navigation.PushPopupAsync(pop2, true);
                            break;
                        case 600:
                            var pop3 = new MessageBox("Email inválido!", "Informe um email verdadeiro!");
                            _ = App.Current.MainPage.Navigation.PushPopupAsync(pop3, true);
                            break;
                        case 300:
                            var pop4 = new MessageBox("Email inválido!", "Informe um email cadastrado em nossa base ou cadastre-se!");
                            _ = App.Current.MainPage.Navigation.PushPopupAsync(pop4, true);
                            break;
                    }



                }
            }


        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(txtEmail.Text))
            //{
            //    var pop = new MessageBox("Informe os Dados!", "Informe o email solicitados no formulario.");
            //    _ = App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
            //}
            //else
            //{
            //    if (TestarInternet())
            //    {
            //        Formulario_Inscricao dados = new Formulario_Inscricao
            //        {
            //            email = txtEmail.Text
            //        };

            //        int resultado = await API_Service.Lembrar_Senha(dados);
            //        switch (resultado)
            //        {
            //            case 1:
            //                var pop = new MessageBox("Email inválido!", "Informe um email verdadeiro!");
            //                _ = App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
            //                break;
            //            case 2:
            //                var pop2 = new MessageBox("Email Inesistente!", "Esta conta de email não está cadastrada em nossos registros, infome um email existente ou realize um cadastro.");
            //                _ = App.Current.MainPage.Navigation.PushPopupAsync(pop2, true);
            //                break;
            //            case 3:
            //                var viewPage = new LoginPage_oficial(false, 0, "", "");
            //                var navigation = Application.Current.MainPage as NavigationPage;
            //                var pop3 = new MessageBox("Lembrete de senha enviado!", string.Format("Um lembrete de senha enviado para o email: '{0}'", txtEmail.Text));
            //                _ = App.Current.MainPage.Navigation.PushPopupAsync(pop3, true);
            //                await navigation.PushAsync(viewPage, true);
            //                break;
            //            default:
            //                break;
            //        }

            //    }
            //}break

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
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


    }
}