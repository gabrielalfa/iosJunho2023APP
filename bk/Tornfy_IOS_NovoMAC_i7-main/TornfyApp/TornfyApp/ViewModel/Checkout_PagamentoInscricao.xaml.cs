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
    public partial class Checkout_PagamentoInscricao : ContentPage
    {

        public Config_Home config;
        public int id_jogador;
        public int id_master;
        public int id_etapa;
        public Etapa etapa;
        public int total_pgo;
        public string lista_idInscricoes;
        public bool pago;
        public string codigo;
        public string charge_codigo;


        public Checkout_PagamentoInscricao(Config_Home Config, int Id_jogador, int Id_master, int id_Etapa, int _total_pgo, string _lista_idInscricoes)
        {

            config = Config;
            id_etapa = id_Etapa;
            id_jogador = Id_jogador;
            id_master = config.master;
            total_pgo = _total_pgo;
            lista_idInscricoes = _lista_idInscricoes;

            InitializeComponent();


            CarregarCreditos(id_jogador, id_etapa);
        }

        public async void CarregarCreditos(int id_jogador, int id_etapa)
        {
            Pagamento retorno = await API_Service.ObterCheckoutPagamento(id_jogador, id_etapa, total_pgo, lista_idInscricoes, id_master);

            if (retorno.status_pagarme)
            {
                img_qrcode.Source = retorno.QrCodeUrl;
                txtcodigo.Text = retorno.QrCode;
                codigo = retorno.codigo;
                charge_codigo = retorno.charge_codigo;
            }
            else
            {
                MessageBox pop = new MessageBox("PIX não gerado!", "PIX de pagamento não foi gerado, favor tentar novamente mais tarde!.");
                await Application.Current.MainPage.Navigation.PushPopupAsync(pop, true).ConfigureAwait(false);
            }


        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Clipboard.SetTextAsync(txtcodigo.Text);
            Mensagem_Simples pop = new Mensagem_Simples("Código PIX copiado!",
                "PIX de pagamento copiado para a área de transferencia." +
                " Realize o pagamento e retorne a esta página para conferir o status de pagamento.", "#3c62aa", "Fechar Mensagem");
            await Application.Current.MainPage.Navigation.PushPopupAsync(pop, true).ConfigureAwait(false);
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            //verificar se existe pagamentos pendentes
            pago = await API_Service.ConferirPagamentoUnico(codigo, charge_codigo);

            if (pago)
            {
                await Navigation.PopToRootAsync();
                Mensagem_Simples pop = new Mensagem_Simples("Pagamento Realizado!",
                "PIX de pagamento foi realizado com sucesso, e baixado nos registros de pagamento da plataforma tornfy.com." +
                " Realize o pagamento e retorne a esta página para conferir o status de pagamento.", "#3c62aa", "Fechar Mensagem");
                await Application.Current.MainPage.Navigation.PushPopupAsync(pop, true).ConfigureAwait(false);
            }
            else
            {
                Mensagem_Simples pop = new Mensagem_Simples("Pagamento ainda não reconhecido!",
                               "PIX de pagamento ainda não foi reconhecido pela plataforma. Se você já pagou realize conferencia em alguns instantes. Tempo estimado da baixa de valores de pagamento é de até 30segundos em média" +
                               " Realize o pagamento e retorne a esta página para conferir o status de pagamento.", "#E10555", "Fechar Mensagem");
                await Application.Current.MainPage.Navigation.PushPopupAsync(pop, true).ConfigureAwait(false);
            }

        }


    }
}