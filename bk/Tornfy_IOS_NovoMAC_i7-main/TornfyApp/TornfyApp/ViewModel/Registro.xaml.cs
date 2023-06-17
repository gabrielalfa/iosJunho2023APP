using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TornfyApp.API;
using TornfyApp.Model;
using Xamarin.Essentials;

namespace TornfyApp.ViewModel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registro : ContentPage
    {
        public string cpf;
        private static readonly Random getrandom = new Random();
        public int ID_Jogador_pos;

        public string id_propietario = "1";

        public Registro(string cpf)
        {
            this.cpf = cpf;

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

        private async void btn_registrar_Clicked(object sender, EventArgs e)
        {
            int id_master = 1;

            //validar campos
            bool validacao = true;
            if (string.IsNullOrEmpty(txtNome.Text)) validacao = false;
            if (string.IsNullOrEmpty(txtSobreNome.Text)) validacao = false;
            if (string.IsNullOrEmpty(txtEmail.Text)) validacao = false;
            if (string.IsNullOrEmpty(txtTelefone.Text)) validacao = false;
            if (string.IsNullOrEmpty(txtSenha.Text)) validacao = false;
            if (string.IsNullOrEmpty(txtNasc.Text)) validacao = false;

            if (TestarInternet())
            {
                //campos em branco
                if (!validacao)
                {
                    var pop = new MessageBox("Informe os Dados!", "Informe todos os dados solicitados!");
                    await App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
                }

                //campos ok
                if (validacao)
                {
                    //valida caracteres de email
                    Regex rg = new Regex(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");

                    //valida email
                    if (!rg.IsMatch(txtEmail.Text.Trim()))
                    {
                        var pop = new MessageBox("Email Inválido!", "Para registrar é necessário informar email válido!");
                        await App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
                    }

                    //verificar no banco de dados a existencia do email
                    bool email_exist = await API_Service.BuscaEmail_BD(txtEmail.Text.Trim(), 1);
                    if (email_exist)
                    {
                        var pop = new MessageBox("Email Existente!", "Este email já existe na base de dados. A duplicidade não é permitida!");
                        await App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
                    }
                    else
                    {
                        int gd = getrandom.Next(111111, 999999);
                        var usuario = new Usuario();

                        usuario.Primeiro_nome = txtNome.Text.Trim();
                        usuario.Segundo_Nome = txtSobreNome.Text.Trim();
                        usuario.Email = txtEmail.Text.Trim();
                        usuario.Telefone = txtTelefone.Text.Trim();
                        usuario.clube_beach = 10;
                        usuario.clube_beach = 10;
                        usuario.id_Clube = 10;
                        usuario.Password = txtSenha.Text.Trim();
                        usuario.CPF = cpf.Trim();
                        usuario.gd = gd.ToString();
                        usuario.img_path = "/Content/images/Jogadores/user_icon_blue.png";
                        usuario.id_master = id_master;
                        usuario.Nascimento = txtNasc.Text.Trim();

                        ID_Jogador_pos = await API_Service.RegistrarJogador(usuario);

                        if (ID_Jogador_pos == 0)
                        {
                            await DisplayAlert("Adição não realizada", "Falha ao salvar jogador!!!", "Fechar");
                            //await Navigation.PopAsync();
                        }
                        else
                        {
                            //await DisplayAlert("Adição realizada", "Registro Salvo com sucesso!!!", "Fechar");
                            //await Navigation.PopAsync();
                        }


                        var viewPage = new LoginPage_oficial(true, ID_Jogador_pos, txtEmail.Text.Trim(), txtSenha.Text.Trim());
                        var navigation = Application.Current.MainPage as NavigationPage;
                        await navigation.PushAsync(viewPage, true);

                        //Validacao_email(txtEmail.Text.Trim(), gd.ToString(), txtSenha.Text.Trim(), ID_Jogador_pos);



                    }
                }
                else _ = Navigation.PushAsync(new NoInternet());

            }

        }

        public async void Validacao_email(string email, string gd, string senha)
        {
            Login form = new Login
            {
                email = email,
                gd = gd,
                senha = senha
            };

            int resultado = await API_Service.Confirmar_Conta(form);
            if (resultado == 200)
            {
                //enviar para validação de codigo de email
                var viewPage = new Confirmar_Codigo(gd.ToString().Trim(), ID_Jogador_pos);
                var navigation = Application.Current.MainPage as NavigationPage;
                await navigation.PushAsync(viewPage, true);
            }
            else
            {
                var pop = new MessageBox("Envio não realizado!", "Houve uma falha no envio do seu email, talvez o email esteja incorreto! Procure o suporte!");
                _ = App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
            }

        }


    }
}