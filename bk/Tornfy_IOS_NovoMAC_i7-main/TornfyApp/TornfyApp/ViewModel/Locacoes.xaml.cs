using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TornfyApp.API;
using TornfyApp.Model;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TornfyApp.ViewModel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Locacoes : ContentPage
    {
        public Config_Home config;
        public string data { get; set; }
        public int id_master;
        public List<Quadra> Lista_quadras;
        public List<Quadra> Lista_pagamentos;
        public List<Quadra> Lista_horarios;
        public int id_quadra_selecionada;
        public int id_jogador;
        public string data_sel;
        public string string_quadra;
        public string nome_locacoes;
        public int id_proprietario;

        public bool mensagem = false;

        public Locacoes(Config_Home Config, int id_master, int id_jogador, int id_quadra_selecionada, string string_quadra, string nome_locacoes, int id_proprietario)
        {
            InitializeComponent();

            ToolbarItems.Remove(cart_shop_ball);
            ToolbarItems.Remove(cart_shop);

            config = Config;
            this.id_master = id_master;
            this.id_jogador = id_jogador;
            this.id_quadra_selecionada = id_quadra_selecionada;
            this.string_quadra = string_quadra;
            this.nome_locacoes = nome_locacoes;
            this.id_proprietario = id_proprietario;

            Title = string_quadra;

            carroucel_days.ItemsSource = GetDays(false, 0);
            this.data_sel = DateTime.Today.ToString();

            //Buscar_Horarios();

            carroucel_horarios.RefreshCommand = new Command(() =>
            {
                Buscar_Horarios();
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() => carroucel_horarios.IsRefreshing = false);
            });


        }

        protected override void OnAppearing()
        {
            ToolbarItems.Remove(cart_shop_ball);
            ToolbarItems.Remove(cart_shop);
            Buscar_Pagamentos();
            if (id_quadra_selecionada != 0)
            {
                Buscar_Horarios();
            }
        }

        public async void Buscar_Horarios()
        {
            Lista_horarios = await API_Service.ObterHorarios(data_sel, id_master, id_quadra_selecionada, id_jogador, nome_locacoes, string_quadra);
            carroucel_horarios.ItemsSource = Lista_horarios.ToList();
        }

        public async void Buscar_Pagamentos()
        {
            Lista_pagamentos = await API_Service.ObterPagamentosQuadra("", "", id_master, id_jogador);

            bool pagamento = false;
            foreach (var item in Lista_pagamentos)
            {
                if (!item.status_pagamento)
                {
                    pagamento = true;
                    break;
                }
            }

            if (pagamento) { ToolbarItems.Add(cart_shop_ball); } else { ToolbarItems.Add(cart_shop); }

            //carroucel_horarios.ItemsSource = Lista_horarios.ToList();
        }

        private ObservableCollection<Calendario> horarios;
        public ObservableCollection<Calendario> horas
        {
            get { return horarios; }
            set
            {
                horarios = value;
                OnPropertyChanged();
            }
        }



        private ObservableCollection<Calendario> calendario;
        public ObservableCollection<Calendario> Days
        {
            get { return calendario; }
            set
            {
                calendario = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Calendario> GetDays(bool selecionado, int day_sel)
        {
            calendario = new ObservableCollection<Calendario>();

            int day_today = DateTime.Today.Day;

           DateTime dateValue = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            string dia_semana = dateValue.ToString("ddd", new CultureInfo("pt-BR"));

           
           


            int day_final = 0;

            DateTime dtAutual = DateTime.Today;
            DateTime dtUltimoDiaMes = new DateTime(dtAutual.Year, dtAutual.Month, DateTime.DaysInMonth(dtAutual.Year, dtAutual.Month));

            for (int i = day_today; i <= dtUltimoDiaMes.Day; i++)
            {
                dateValue = new DateTime(DateTime.Today.Year, DateTime.Today.Month, i);
                dia_semana = dateValue.ToString("ddd", new CultureInfo("pt-BR"));

                if (selecionado)
                {
                    if (i == day_sel)
                    {
                        calendario.Add(new Calendario()
                        {
                            string_semana = dia_semana.ToUpper(),
                            dia = i,
                            Back_Color = "#3c62aa",
                            Color_Font = "#fff",
                            data_sel = dateValue.ToString("dd/MM/yyyy", new CultureInfo("pt-BR")),
                        });
                    }
                    else
                    {
                        calendario.Add(new Calendario()
                        {
                            string_semana = dia_semana.ToUpper(),
                            dia = i,
                            Back_Color = "#dedede",
                            Color_Font = "#000",
                            data_sel = dateValue.ToString("dd/MM/yyyy", new CultureInfo("pt-BR")),
                        });
                    }
                }
                else
                {
                    if (i == day_today)
                    {
                        calendario.Add(new Calendario()
                        {
                            string_semana = dia_semana.ToUpper(),
                            dia = i,
                            Back_Color = "#3c62aa",
                            Color_Font = "#fff",
                            data_sel = dateValue.ToString("dd/MM/yyyy", new CultureInfo("pt-BR")),
                        });
                    }
                    else
                    {
                        calendario.Add(new Calendario()
                        {
                            string_semana = dia_semana.ToUpper(),
                            dia = i,
                            Back_Color = "#dedede",
                            Color_Font = "#000",
                            data_sel = dateValue.ToString("dd/MM/yyyy", new CultureInfo("pt-BR")),
                        });
                    }
                }



            }

            int difDays = dtUltimoDiaMes.Day - day_today;
            difDays = 7 - difDays;
            if (difDays <= 7)
            {
                for (int i = 1; i <= difDays; i++)
                {
                    dateValue = new DateTime(DateTime.Today.Year, DateTime.Today.Month + 1, i);
                    dia_semana = dateValue.ToString("ddd", new CultureInfo("pt-BR"));

                    if (i == day_sel)
                    {
                        calendario.Add(new Calendario()
                        {
                            string_semana = dia_semana.ToUpper(),
                            dia = i,
                            Back_Color = "#3c62aa",
                            Color_Font = "#fff",
                            data_sel = dateValue.ToString("dd/MM/yyyy", new CultureInfo("pt-BR")),
                        });
                    }
                    else
                    {
                        calendario.Add(new Calendario()
                        {
                            string_semana = dia_semana.ToUpper(),
                            dia = i,
                            Back_Color = "#dedede",
                            Color_Font = "#000",
                            data_sel = dateValue.ToString("dd/MM/yyyy", new CultureInfo("pt-BR")),
                        });
                    }
                }
            }

            return calendario;
        }



        private async void carroucel_days_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var calendario = e.CurrentSelection[0] as Calendario;
            this.data_sel = calendario.data_sel.ToString().Substring(0, 10);

           
            carroucel_days.ItemsSource = GetDays(true, calendario.dia);
            //carroucel_horarios.ItemsSource = GetHorarios(data);
            Buscar_Horarios();

            var actions = new SnackBarActionOptions
            {
                Action = () => DisplayAlert("Lista atualizada", "Faça sua locação.", "OK"),
                Text = "Fechar"
            };

            var option = new SnackBarOptions
            {
                MessageOptions = new MessageOptions
                {
                    Foreground = Color.White,
                    Message = string.Format("Horários atualizados com sucesso para a data selecionada: {0}!", data_sel),
                    Padding = 10
                },
                BackgroundColor = Color.FromHex("#2E4159"),
                Duration = TimeSpan.FromSeconds(2),

                //Actions = new[] { actions }


            };

            await this.DisplaySnackBarAsync(option);

            if (calendario == null)
            {
                return;
            }
        }


        private async void ingredientsButton_Clicked(object sender, EventArgs e)
        {
            await OpenBrowser("https://api.whatsapp.com/send?phone=555497142091&text=Gostaria%20de%20mais%20informa%C3%A7%C3%B5es%20sobre%20a%20plataforma%20ICT.");
        }

        public async Task OpenBrowser(string uri)
        {
            try
            {
                await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception)
            {
                // An unexpected error occured. No browser may be installed on the device.
            }
        }

        private void carroucel_horarios_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private async void carroucel_horarios_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            ((ListView)sender).SelectedItem = null;

        }



        private void pckQuadra_SelectedIndexChanged(object sender, EventArgs e)
        {

            Quadra quadra = ((Picker)sender).SelectedItem as Quadra;
            if (quadra == null)
            {
                return;
            }
            id_quadra_selecionada = quadra.id;
            Buscar_Horarios();

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            mensagem = true;

            Quadra quadra = ((Button)sender).BindingContext as Quadra;
            if (quadra == null)
            {
                return;
            }

            id_quadra_selecionada = quadra.id_quadra;
            int id_locacao = quadra.id;

            if (quadra.status)
            {
                //se a locação pertencer ao jogador logado
                if (quadra.id_jogador == id_jogador)
                {

                    //locar quadra
                    bool result = await API_Service.Liberar_Quadra(quadra.id, quadra.id_quadra, id_master, quadra.data);

                    if (result)
                    {
                        var pop = new Mensagem_Simples("Liberação da Quadra realizada com sucesso!", "Cobranças relativas a essa locação podem ou não ser feitas pela secretaria do seu clube. Dúvidas pergunte ao clube responsável.", "#E10555", "Fechar Mensagem");
                        _ = App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
                    }
                    else
                    {
                        var pop = new Mensagem_Simples("Liberação da Quadra não realizada!", "A desmarcação desta quadra conflita com as regras de marcação e desmarcação deste local.", "#E10555", "Fechar Mensagem");
                        _ = App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
                    }

                    Buscar_Horarios();

                }
                else
                {
                    //caso a locação não pertença ao jogador logado 
                    var pop = new Mensagem_Simples("Liberação não permitida!", "Liberação não permitida. A locação que você está tentando liberar não pertence ao seu usuário.", "#E10555", "Fechar Mensagem");
                    _ = App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
                }

            }
            else
            {



                int result = await API_Service.Locar_Quadra(id_locacao, id_jogador, id_master, "40", id_proprietario);

                if (result == 0)
                {
                    //futuramente implantação de pagamentos multiplos
                    string lista_pagamentos = "_" + quadra.id;

                    //locar quadra
                    var pop = new Mensagem_Simples("Locação da Quadra realizada com sucesso!",
                        "Clique no carrinho de compras para realizar o pagamento.", "#3c62aa", "Fechar Mensagem");
                    _ = App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
                    //await Navigation.PushAsync(new Fechamento_PagamentoLocacao(config, id_jogador, quadra.id_quadra, quadra.id, lista_pagamentos, id_master));

                    ToolbarItems.Remove(cart_shop);
                    ToolbarItems.Remove(cart_shop_ball);
                    ToolbarItems.Add(cart_shop_ball);


                }

                if (result == 7)
                {

                    decimal valor_cobranca = 0;
                    try
                    {
                        valor_cobranca = decimal.Parse(quadra.valor_iluminacao) + decimal.Parse(quadra.valor_quadra);
                    }
                    catch (Exception)
                    { }
                    await Navigation.PushAsync(new Rateio(config, id_jogador, id_master,
                        valor_cobranca, string_quadra, quadra.data, quadra.horario, id_locacao));
                }

                if (result == 1)
                {
                    Buscar_Horarios();
                    var pop = new Mensagem_Simples("Locação da Quadra não foi realizada!", "Uma marcação de outro usuário antecedeu a sua marcação. Encontre outra quadra para marcação.", "#E10555", "Fechar Mensagem");
                    _ = App.Current.MainPage.Navigation.PushPopupAsync(pop, true);

                }

                if (result == 3)
                {
                    Buscar_Horarios();
                    var pop = new Mensagem_Simples("Locação da Quadra não foi realizada!", "Horário escolhido é anterior ao presente, escolha um horário futuro.", "#E10555", "Fechar Mensagem");
                    _ = App.Current.MainPage.Navigation.PushPopupAsync(pop, true);

                }

                if (result == 4)
                {
                    Buscar_Horarios();
                    var pop = new Mensagem_Simples("Locação da Quadra não foi realizada!", "Data excede limite de janela de dias deteminada pelo organizador.", "#E10555", "Fechar Mensagem");
                    _ = App.Current.MainPage.Navigation.PushPopupAsync(pop, true);

                }

                if (result == 5)
                {
                    Buscar_Horarios();
                    var pop = new Mensagem_Simples("Locação da Quadra não foi realizada!", "Quantidade de locações por usuários por dia excede o limite determinado pelo administrador.", "#E10555", "Fechar Mensagem");
                    _ = App.Current.MainPage.Navigation.PushPopupAsync(pop, true);

                }

                if (result == 6)
                {
                    Buscar_Horarios();
                    var pop = new Mensagem_Simples("Locação da Quadra não foi realizada!", "Quantidade de locações por usuários por semana excede o limite determinado pelo administrador.", "#E10555", "Fechar Mensagem");
                    _ = App.Current.MainPage.Navigation.PushPopupAsync(pop, true);

                }

                if (result == 500)
                {
                    Buscar_Horarios();
                    var pop = new Mensagem_Simples("Falha de Comunicação!", "Falha na comunicação com a API.", "#E10555", "Fechar Mensagem");
                    _ = App.Current.MainPage.Navigation.PushPopupAsync(pop, true);

                }

                Buscar_Horarios();
            }

        }


        private async void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Fechamento_PagamentoLocacao(config, id_jogador, id_master, "", false, 0, 0, false, 0));
        }

        async void cart_shop_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Fechamento_PagamentoLocacao(config, id_jogador, id_master, "", false, 0, 0, false, 0));
        }
    }
}
