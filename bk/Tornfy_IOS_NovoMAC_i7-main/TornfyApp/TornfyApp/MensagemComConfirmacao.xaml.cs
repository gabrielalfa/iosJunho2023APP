using API_Inteleco.Models;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TornfyApp.API;
using TornfyApp.Model;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TornfyApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MensagemComConfirmacao : PopupPage
    {
        public bool confirmacao;
        public string telefone;
        public string mensagem;
        public string email;
        public bool envio_email;
        public int id_jogador;
        public int id_complexo;


        public MensagemComConfirmacao(string titulo, string mensagem,
            string color_btn, string text_btn, bool confirmacao, string telefone,
            bool envio_email, string email, int id_complexo)
        {
            InitializeComponent();

            CarregarSecureStorage();
            txt_titulo.Text = titulo;
            txt_mensagem.Text = mensagem;
            btn_close.BackgroundColor = Color.FromHex(color_btn.ToString());
            btn_close.Text = text_btn;
            this.confirmacao = confirmacao;
            this.telefone = telefone;
            this.mensagem = mensagem;
            this.email = email;
            this.envio_email = envio_email;
            this.id_complexo = id_complexo;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (confirmacao)
            {
                string telefoneCompleto = telefone;
                if (!telefoneCompleto.StartsWith("55"))
                {
                    telefoneCompleto = "55" + Regex.Replace(telefoneCompleto, "[^0-9]", ""); ;
                }

                //string url = string.Format("https://api.whatsapp.com/send?phone={0}&text={1}", telefoneCompleto, mensagem);
                //await Browser.OpenAsync(url, BrowserLaunchMode.SystemPreferred);

                //enviar email
                if (envio_email)
                {
                    EnviarEmail();
                    await DisplayAlert("Aviso de Sistema!", "Fique tranquilo, " +
                      string.Format("enviamos um email para o organizador avisando " +
                      "da sua habilitação, aguarde o retorno em breve. Se necessário entre em contato: {0}", telefone), "fechar");
                }

            }
            //fecha a janela
            await App.Current.MainPage.Navigation.PopPopupAsync(true);

        }

        public async void EnviarEmail()
        {
            Quadra dados = new Quadra();
            dados.id_jogador = id_jogador;
            dados.email = email;
            dados.id_complexo = id_complexo;
            int retorno = await API_Service.EnviarEmail(dados);
            if (retorno == 200)
            {
                //200 envio ok

            }

        }

        public async void CarregarSecureStorage()
        {
            string id_jogador_storege = await Xamarin.Essentials.SecureStorage.GetAsync("id_jogador");
            if (!string.IsNullOrEmpty(id_jogador_storege)) this.id_jogador = int.Parse(id_jogador_storege);


        }
    }
}