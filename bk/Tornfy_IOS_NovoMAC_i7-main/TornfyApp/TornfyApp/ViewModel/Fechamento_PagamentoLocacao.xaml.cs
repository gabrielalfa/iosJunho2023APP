using Acr.UserDialogs.Extended;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TornfyApp.API;
using TornfyApp.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TornfyApp.ViewModel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Fechamento_PagamentoLocacao : ContentPage
    {
        public Config_Home config;
        public int id_quadra;
        public int id_jogador;
        public int id_locacao;
        public decimal valor_rateio;
        public int id_master;
        public bool rateio;
        public string _lista_pagamentos;
        List<Quadra> pagamentos;
        public List<Quadra> Lista_pagamentos;

        public string lista_rateio;
        public decimal valor_original;
        public bool pgo_integral;

        public string inicio = "";
        public string fim = "";

        public Fechamento_PagamentoLocacao(Config_Home Config, int id_jogador, int id_master,
            string lista_rateio, bool rateio, decimal valor_rateio,
            decimal valor_original, bool pgo_integral, int id_locacao)
        {
            InitializeComponent();

            config = Config;
            this.id_jogador = id_jogador;
            this.id_master = id_master;
            this.id_locacao = id_locacao;
            this.valor_rateio = valor_rateio;
            this.rateio = rateio;
            this.lista_rateio = lista_rateio;
            this.valor_original = valor_original;
            this.pgo_integral = pgo_integral;

            lista_pagamentos.RefreshCommand = new Command(() =>
            {
                Buscar_Pagamentos();
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() => lista_pagamentos.IsRefreshing = false);
            });

        }

        protected override void OnAppearing()
        {
            Buscar_Pagamentos();
        }

        public async void Buscar_Pagamentos()
        {
            
            Lista_pagamentos = await API_Service.ObterPagamentosQuadra(inicio, fim, id_master, id_jogador);


            Credito credito = await API_Service.ObterCreditoLocacao(id_master, id_jogador);

            decimal soma_quadra = 0;
            decimal soma_iluminacao = 0;
            decimal total_pgo = 0;


            foreach (var item in Lista_pagamentos)
            {

                item.data = item.data + " - " + item.horario;

                if (item.status_pagamento)
                {
                    item.status_pgo_final = "pago"; item.color_valor = "#3c62aa";
                }
                else
                {

                    //rateio
                    if (rateio)
                    {
                        if (item.id == id_locacao)
                        {
                            item.status_pgo_final = "Aguar.Pagamento"; item.color_valor = "#E10555";
                            total_pgo += valor_rateio;

                            item.valor_cobranca = valor_rateio.ToString();

                            _lista_pagamentos += "_" + item.id;
                        }
                        else
                        {
                            item.status_pgo_final = "Aguar.Pagamento"; item.color_valor = "#E10555";

                            soma_quadra = decimal.Parse(item.valor_quadra);
                            soma_iluminacao = decimal.Parse(item.valor_iluminacao);
                            total_pgo += decimal.Parse(item.valor_cobranca);

                            _lista_pagamentos += "_" + item.id;
                        }



                    }
                    else
                    {
                        item.status_pgo_final = "Aguar.Pagamento"; item.color_valor = "#E10555";

                        soma_quadra = decimal.Parse(item.valor_quadra);
                        soma_iluminacao = decimal.Parse(item.valor_iluminacao);
                        total_pgo += decimal.Parse(item.valor_cobranca);

                        _lista_pagamentos += "_" + item.id;
                    }


                }
            }

            valor_pgo.Text = total_pgo.ToString("N2", CultureInfo.GetCultureInfo("pt-BR"));

            if (total_pgo == 0)
            {
                btn_pgoPix.IsVisible = false;
                btn_naoValores.IsVisible = true;
                
                btn_credito.IsVisible = false;

            }
            else
            {
                if (credito.total_creditos != 0)
                {
                    btn_pgoPix.IsVisible = false;
                    btn_credito.IsVisible = true;
                    btn_credito.Text = string.Format("Pagar com Credito (R$ {0})", credito.total_creditos.ToString());
                }
                else
                {
                    btn_credito.IsVisible = false;
                    btn_pgoPix.IsVisible = true;
                }


                btn_naoValores.IsVisible = false;
                
            }

            lista_pagamentos.ItemsSource = Lista_pagamentos.ToList().OrderBy(x => x.data);
        }



        private void lista_pagamentos_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private void lista_pagamentos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }


        private async void btn_naoValores_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
            await Navigation.PushAsync(new MasterPage(config, id_jogador, 1), true);
        }

        private async void btn_pgoPix_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Checkout_PagamentoLocacao(
                config, id_jogador, valor_pgo.Text.ToString(), _lista_pagamentos, id_master,
                lista_rateio, rateio, valor_rateio, valor_original, pgo_integral, id_locacao));
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            inicio = startDatePicker.Date.ToString("dd/MM/yyyy");
            fim = endDatePicker.Date.ToString("dd/MM/yyyy");

            Buscar_Pagamentos();
        }

        private async void btn_credito_Clicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading(title: "Atualizando Pagamento");
            int retorno = await API_Service.DescontarCreditoLocacao(id_master, id_jogador);

            switch (retorno)
            {
                case 200:   
                    Buscar_Pagamentos();
                    break;
                case 500:
                    var pop = new MessageBox("Falha de Pagamento!", "Houve uma falha nos pagamentos, iremos resolver o mais breve possível, tente novamente mais tarde!");
                    _ = App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
                    break;
                default:
                    break;

            }

            UserDialogs.Instance.HideLoading();

        }
    }
}