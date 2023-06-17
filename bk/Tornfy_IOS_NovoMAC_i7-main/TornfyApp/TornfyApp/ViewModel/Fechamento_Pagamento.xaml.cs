using Acr.UserDialogs.Extended;
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
    public partial class Fechamento_Pagamento : ContentPage
    {
        public Config_Home config;
        public List<Inscricao> Lista_incricoes;
        public List<Inscricao> Lista_credito;
        public int id_jogador;
        public int id_master;
        public int id_etapa;

        public decimal somatotal;
        public decimal pagas;
        public int total_pgo;
        decimal soma_credito;
        public int id_tipo;

        public Etapa info_etapa;
        string imagem_cartaz = "";
        string nome_etapa = "";
        public string lista_inscricoes;


        public Fechamento_Pagamento(Config_Home Config, int Id_jogador, int Id_master, int id_Etapa, int _id_tipo)
        {
            if (TestarInternet()) CarregarInscricoes(Id_jogador, Id_master, id_Etapa);
            else
                Navigation.PushAsync(new NoInternet());
            if (TestarInternet()) CarregarCreditos(Id_jogador, id_etapa);
            else
                Navigation.PushAsync(new NoInternet());

            InitializeComponent();


            config = Config;
            this.id_etapa = id_Etapa;
            id_jogador = Id_jogador;
            id_tipo = _id_tipo;
            txt_vazio.IsVisible = false;
            id_master = Id_master;

            if (total_pgo == 0)
            {
                btn_pgoPix.IsVisible = false;
                btn_naoValores.IsVisible = true;
            }
            else
            {
                btn_pgoPix.IsVisible = true;
                btn_naoValores.IsVisible = false;
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (TestarInternet()) CarregarInscricoes(id_jogador, config.master, id_etapa);
            if (TestarInternet()) CarregarCreditos(id_jogador, id_etapa);

        }


        public async void CarregarInscricoes(int id_jogador, int id_master, int id_etapa)
        {
            decimal somatotal = 0;
            decimal somatotal1 = 0;
            decimal pagas = 0;
            decimal total_pgo = 0;
            decimal valor_temp = 0;
            decimal valor_temp1 = 0;





            Lista_credito = await API_Service.ObterInscricoes_Creditos(id_jogador, this.id_etapa);

            soma_credito = 0;

            foreach (var item in Lista_credito)
            {
                soma_credito = soma_credito + decimal.Parse(item.saldo);
                btn_pgoCredito.Text = "Usar Crédito R$" + soma_credito;
            }

            UserDialogs.Instance.ShowLoading(title: "Buscando Lista");

            Lista_incricoes = await API_Service.ObterInscricoes_Jogador_etapa(id_jogador, id_master, id_etapa);

            BindingContext = Lista_incricoes.ToList();

            if (Lista_incricoes.Count == 0)
            {
                lista_pagamentos.ItemsSource = Lista_incricoes.ToList();
                txt_vazio.IsVisible = true;
                UserDialogs.Instance.HideLoading();
            }

            lista_pagamentos.ItemsSource = Lista_incricoes.ToList();

            //bool dupla = false;

            foreach (var item in Lista_incricoes)
            {
                //if (item.id_jogador == id_jogador) { dupla = false; item.is_dupla = false; }
                //if (item.id_dupla == id_jogador) { dupla = true; item.is_dupla = true; }

                if (item.is_dupla)
                {
                    if (!item.status_pagamento_dupla)
                    {
                        if (item.valor_dupla == "")
                        {
                            valor_temp = 0;
                        }
                        else
                        {
                            valor_temp = decimal.Parse(item.valor_dupla.Replace('.', ','));
                        }

                        somatotal = somatotal + valor_temp;

                        item.status_pgo_final = "pendente";
                        item.color = "#f56e02";
                    }

                }
                else
                {
                    if (!item.status_pagamento)
                    {
                        if (item.valor_transaction == "")
                        {
                            valor_temp = 0;
                        }
                        else
                        {
                            valor_temp = decimal.Parse(item.valor_transaction.Replace('.', ','));
                        }

                        somatotal = somatotal + valor_temp;

                        item.status_pgo_final = "pendente";
                        item.color = "#f56e02";

                    }


                }



            }

            foreach (var item in Lista_incricoes)
            {
                //if (item.id_jogador == id_jogador) { dupla = false; item.is_dupla = false; }
                //if (item.id_dupla == id_jogador) { dupla = true; item.is_dupla = true; }

                if (item.is_dupla)
                {
                    if (item.status_pagamento_dupla)
                    {
                        if (item.valor_dupla == "")
                        {
                            valor_temp1 = 0;
                        }
                        else
                        {
                            valor_temp1 = decimal.Parse(item.valor_dupla.Replace('.', ','));
                        }

                        somatotal1 = somatotal1 + valor_temp1;

                        item.status_pgo_final = "pago";
                        item.color = "#087fdb";
                    }

                }
                else
                {
                    if (item.status_pagamento)
                    {
                        if (item.valor_transaction == "")
                        {
                            valor_temp1 = 0;
                        }
                        else
                        {
                            valor_temp1 = decimal.Parse(item.valor_transaction.Replace('.', ','));
                        }

                        somatotal1 = somatotal1 + valor_temp1;

                        item.status_pgo_final = "pago";
                        item.color = "#087fdb";

                    }

                }

            }

            decimal total = (somatotal + somatotal1) - somatotal1;
            //desconta credito
            //total = total - soma_credito;

            somatotal = somatotal + somatotal1;
            pagas = somatotal1;
            total_pgo = total;
            this.total_pgo = int.Parse(total.ToString());

            valor_pgo.Text = total_pgo.ToString();

            if (total_pgo == 0)
            {

                btn_pgoCredito.IsVisible = false;
                btn_pgoPix.IsVisible = false;
                btn_naoValores.IsVisible = true;

            }
            else
            {
                if (soma_credito != 0)
                {
                    btn_pgoCredito.IsVisible = true;
                    btn_pgoPix.IsVisible = false;
                    btn_naoValores.IsVisible = false;
                }
                else
                {
                    btn_pgoPix.IsVisible = true;
                    btn_naoValores.IsVisible = false;
                    btn_pgoCredito.IsVisible = false;
                }
            }

            UserDialogs.Instance.HideLoading();




        }

        public async void CarregarCreditos(int id_jogador, int id_etapa)
        {

            Lista_credito = await API_Service.ObterInscricoes_Creditos(id_jogador, this.id_etapa);

            soma_credito = 0;

            foreach (var item in Lista_credito)
            {
                soma_credito = soma_credito + decimal.Parse(item.saldo);
                btn_pgoCredito.Text = "Usar Crédito R$" + soma_credito;
            }

            if (soma_credito == 0)
            {
                // ViewBag.credito_zero = "true";
            }
            else
            {
                //tem credito, usar o mesmo
                //ViewBag.credito_zero = "false";
            }


        }

        private void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs e)
        {
            Button button = (sender as Grid).Children.FirstOrDefault(c => Grid.GetRow(c) == 0 && Grid.GetColumn(c) == 1) as Button;
            button.IsVisible = true;
        }



        private void lista_pagamentos_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private void lista_pagamentos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        public async void CarregarInfoEtapa()
        {
            info_etapa = await API_Service.ObterInfoEtapa(id_etapa);
            nome_etapa = info_etapa.Nome_Etapa;
            config.nome_etapa = nome_etapa;
            if (info_etapa != null)
            {
                await Navigation.PushAsync(new TB_TorneiosPage(nome_etapa, id_etapa, id_jogador, id_tipo, false, info_etapa.publico_programacao, info_etapa.publico_anexo, info_etapa.nome_anexo, info_etapa.nome_principal));
            }

        }

        private void txt_plus_Clicked(object sender, EventArgs e)
        {
            CarregarInfoEtapa();
        }

        private void btn_pgoPix_Clicked(object sender, EventArgs e)
        {
            foreach (var item in Lista_incricoes)
            {
                lista_inscricoes += "#" + item.id.ToString();
            }

            Navigation.PushAsync(new Checkout_PagamentoInscricao(config, id_jogador, id_master, id_etapa, int.Parse(valor_pgo.Text), lista_inscricoes));
        }


        private async void btn_paguei_Clicked_1(object sender, EventArgs e)
        {
            OnAppearing();
            MessageBox pop = new MessageBox("Pagamentos atualizados!", "Caso o status de pagamento ainda não mudou, aguarde mais uns instantes e consulte novamente (tempo médio de 30seg para mudança de status, após pagamento)!");
            await Application.Current.MainPage.Navigation.PushPopupAsync(pop, true).ConfigureAwait(false);
        }


        private async void DeleteMenuItem_Clicked(object sender, EventArgs e)
        {

            MessageBox pop;
            var response = await DisplayAlert("Deletar Inscrição", "Tem certeza que deseja deletar?", "Sim", "Não", FlowDirection.LeftToRight);
            if (response)
            {
                string id = ((MenuItem)sender).CommandParameter.ToString();
                Formulario_Inscricao formulario_Inscricao = new Formulario_Inscricao();
                int response_api = await API_Service.DeletarInscricao(int.Parse(id), formulario_Inscricao);

                //bool isDelete = await studentRepository.Delete(id);
                if (response_api == 200)
                {
                    OnAppearing();
                    pop = new MessageBox("Sucesso!", "Inscrição deletada com sucesso!");
                    await Application.Current.MainPage.Navigation.PushPopupAsync(pop, true).ConfigureAwait(false);

                }
                else
                {
                    pop = new MessageBox("Falha!", "Inscrição não pode ser deletada, pois consta no chaveamento da etapa!");
                    await Application.Current.MainPage.Navigation.PushPopupAsync(pop, true).ConfigureAwait(false);
                }
            }
        }

        private async void btn_naoValores_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
            await Navigation.PushAsync(new MasterPage(config, id_jogador, 1), true);
        }

        private async void btn_pgoCredito_Clicked(object sender, EventArgs e)
        {
            //dar baixa nos creditos

            Etapa dados = new Etapa
            {
                id_jogador = id_jogador,
                id_etapa = id_etapa,
                total_pgo = total_pgo,
                id_master = id_master,
                valor_credito = int.Parse(this.soma_credito.ToString()),
            };

            int result = await API_Service.BaixaCredito(dados);
            if (result == 200)
            {
                if (TestarInternet()) CarregarInscricoes(id_jogador, config.master, id_etapa);
                var pop = new Mensagem_Simples("Uso de crédito realizado!", "O uso deste crédito foi realizado com sucesso.", "#3c62aa", "Fechar Mensagem");
                _ = App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
            }
            else
            {
                    var pop = new Mensagem_Simples("Uso de crédito não realizado!", "O uso deste crédito não foi realizado, acesse tornfy.com e tente novamente via web.", "#E10555", "Fechar Mensagem");
                    _ = App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
                if (TestarInternet()) CarregarInscricoes(id_jogador, config.master, id_etapa);
            }
        }
    }
}