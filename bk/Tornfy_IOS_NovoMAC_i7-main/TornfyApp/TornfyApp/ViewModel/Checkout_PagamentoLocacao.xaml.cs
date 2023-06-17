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
    public partial class Checkout_PagamentoLocacao : ContentPage
    {
        public Config_Home config;
        public int id_jogador;
        public string total_pgo;
        public string lista_pagamentos;
        public int id_master;
        public bool pago;
        public string codigo;

        public string lista_rateio;
        public bool rateio;
        public decimal valor_rateio;
        public decimal valor_original;
        public bool pgo_integral;
        public int id_locacao;

        public Checkout_PagamentoLocacao(Config_Home Config, int id_jogador, string total_pgo, string lista_pagamentos, int id_master,
            string lista_rateio, bool rateio, decimal valor_rateio,
            decimal valor_original, bool pgo_integral, int id_locacao)
        {
            config = Config;
            this.id_jogador = id_jogador;
            this.total_pgo = total_pgo;
            this.lista_pagamentos = lista_pagamentos;
            this.id_master = id_master;


            this.lista_rateio = lista_rateio;
            this.rateio = rateio;
            this.valor_rateio = valor_rateio;
            this.valor_original = valor_original;
            this.pgo_integral = pgo_integral;
            this.id_locacao = id_locacao;

            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            CarregarCreditos();
        }

        public async void CarregarCreditos()
        {
            if (rateio)
            {
                //cobrança com rateio incluso
                Quadra retorno = await API_Service.ObterCheckoutLocacaoRateio(id_jogador, total_pgo, lista_pagamentos, id_master,
                    lista_rateio, rateio, valor_rateio, valor_original, pgo_integral);

                if (retorno.status_pagarme)
                {
                    img_qrcode.Source = retorno.qrcode;
                    txtcodigo.Text = retorno.qr_linha;
                    codigo = retorno.codigo;

                    await DisplayAlert("PIX com Rateio!", "As demais cobranças foram enviadas para os outros jogadores por email.", "pagar PIX");
                }
                else
                {
                    MessageBox pop = new MessageBox("PIX não gerado!", "PIX de pagamento não foi gerado, favor tentar novamente mais tarde!");
                    await Application.Current.MainPage.Navigation.PushPopupAsync(pop, true).ConfigureAwait(false);
                }

            }
            else
            {
                Quadra retorno = await API_Service.ObterCheckoutLocacao(id_jogador, total_pgo, lista_pagamentos, id_master);

                if (retorno.status_pagarme)
                {
                    img_qrcode.Source = retorno.qrcode;
                    txtcodigo.Text = retorno.qr_linha;
                    codigo = retorno.codigo;
                }
                else
                {
                    MessageBox pop = new MessageBox("PIX não gerado!", "PIX de pagamento não foi gerado, favor tentar novamente mais tarde!.");
                    await Application.Current.MainPage.Navigation.PushPopupAsync(pop, true).ConfigureAwait(false);
                }
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
            pago = await API_Service.ConferirPagamentoLocacao(codigo, id_master);

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