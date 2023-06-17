using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TornfyApp.API;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TornfyApp.ViewModel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfirmarCPF : ContentPage
    {
        public ConfirmarCPF()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var viewPage = new LoginPage_oficial(false, 0, "", "");
            var navigation = Application.Current.MainPage as NavigationPage;
            navigation.PushAsync(viewPage, true);
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

        //validar CPF na API
        private async void btn_verificaCPF_Clicked(object sender, EventArgs e)
        {
            if (TestarInternet())
            {
                if (!string.IsNullOrEmpty(txtCPF_1.Text))
                {
                    if (IsCpf(txtCPF_1.Text))
                    {
                        if (txtCPF_1.Text != "000.000.000-00")
                        {
                            //verificar no banco de dados a existencia do CPF
                            bool cpf_exist = await API_Service.BuscaCPF_BD(txtCPF_1.Text, 1);
                            if (!cpf_exist)
                            {
                                var viewPage = new Registro(txtCPF_1.Text);
                                var navigation = Application.Current.MainPage as NavigationPage;
                                await navigation.PushAsync(viewPage, true);
                            }
                            else
                            {
                                //caso CPF já exista na base de dados
                                var pop = new MessageBox("CPF Já Existente!", "Não é permitida da duplicidade de CPF na Base de dados!");
                                await App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
                            }
                        }
                        else
                        {
                            //caso CPF não seja validado pelo método IsCpf
                            var pop = new MessageBox("CPF Inválido!", "Para registra é necessário informar CPF válido!");
                            await App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
                        }

                    }
                    else
                    {
                        //caso CPF não seja validado pelo método IsCpf
                        var pop = new MessageBox("CPF Inválido!", "Para registrar é necessário informar CPF válido!");
                        await App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
                    }
                }
                else
                {
                    //caso CPF não seja validado pelo método IsCpf
                    var pop = new MessageBox("CPF Inválido!", "Para registrar é necessário informar CPF válido!");
                    await App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
                }
            }
        }

        public bool IsCpf(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }


    }
}