using Acr.UserDialogs.Extended;
using Foundation;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TornfyApp.API;
using TornfyApp.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TornfyApp.ViewModel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Incricao_Torneio : ContentPage
    {
        public Etapa info_etapa;
        int id_etapa = 0;
        int id_jogador = 0;
        string imagem_cartaz = "";
        string nome_etapa = "";
        public Config_Home config;
        public bool publico;
        public Jogador info_jogador;
        public string nome_jogador;
        public int id_tipo;
        public int id_master;

        public Etapa SelectedBurger { get; internal set; }
        public int Position { get; internal set; }

        private NSTimer timer;

        public Incricao_Torneio(string nome_etapa, int id, int id_jogador, int _id_tipo, int id_master)
        {
            InitializeComponent();

            id_etapa = id;
            this.id_jogador = id_jogador;
            this.id_master = id_master;
            this.id_tipo = _id_tipo;
            Title = nome_etapa;



        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CarregarInfoEtapa();
            CarregarDadosJogador(id_jogador);

            timer = NSTimer.CreateRepeatingScheduledTimer(10, TimerCallback);
        }

        private void TimerCallback(NSTimer timer)
        {

            CarregarInfoEtapaBotao();
        }

        public async void CarregarDadosJogador(int id_jogador)
        {
            info_jogador = await API_Service.ObterDadosJogador(id_jogador);
            this.nome_jogador = info_jogador.Nome_Jogador;
        }


        public async void CarregarInfoEtapaBotao()
        {

            info_etapa = await API_Service.ObterInfoEtapa(id_etapa);

            if (info_etapa.publico)
            {
                publico = true;
                if (info_etapa.andamento)
                {
                    btnLogin.BackgroundColor = Color.FromHex("#3c62aa");
                    btnLogin.Text = "Torneio em Andamento";
                }
                else
                {
                    btnLogin.BackgroundColor = Color.FromHex("#3c62aa");
                    btnLogin.Text = "Inscrever";
                }
            }
            else
            {
                publico = false;
                btnLogin.BackgroundColor = Color.FromHex("#EC407A");
                btnLogin.Text = "Incrição Encerrada";
            }

        }


        public async void CarregarInfoEtapa()
        {


            try
            {
                info_etapa = await API_Service.ObterInfoEtapa(id_etapa);

                path_img.Source = info_etapa.path_clube;
                image_cartaz.Source = info_etapa.img_path;

                imagem_cartaz = info_etapa.img_path;

                txt_NomeEtapa.Text = info_etapa.Nome_Etapa;
                txt_NomeClube.Text = "(" + info_etapa.Nome_clube + ") " + info_etapa.string_clube;
                this.nome_etapa = info_etapa.Nome_Etapa;
                lbl_circuito.Text = info_etapa.nome_grupo;

                if (info_etapa.publico)
                {




                    publico = true;
                    if (info_etapa.andamento)
                    {
                        btnLogin.BackgroundColor = Color.FromHex("#3c62aa");
                        btnLogin.Text = "Torneio em Andamento";
                    }
                    else
                    {
                        btnLogin.BackgroundColor = Color.FromHex("#3c62aa");
                        btnLogin.Text = "Inscrever";
                    }
                }
                else
                {
                    publico = false;
                    btnLogin.BackgroundColor = Color.FromHex("#EC407A");
                    btnLogin.Text = "Incrição Encerrada";
                }

            }
            catch (Exception)
            {

            }



        }

        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new Mostrar_Imagem(imagem_cartaz));


        }

        private async void btnLogin_Clicked(object sender, EventArgs e)
        {

            info_etapa = await API_Service.ObterInfoEtapa(id_etapa);

            if (info_etapa.publico)
            {
                await Navigation.PushAsync(new Categorias_Inscricao(id_master, id_etapa, id_jogador, this.nome_etapa, this.nome_jogador, this.id_tipo));
            }
            else
            {
                publico = false;
                btnLogin.BackgroundColor = Color.FromHex("#EC407A");
                btnLogin.Text = "Incrição Encerrada";

                var pop = new MessageBox("Inscrição Encerrada!", "A inscrição desta etapa encontrar-se encerrada!");
                await Application.Current.MainPage.Navigation.PushPopupAsync(pop, true).ConfigureAwait(false);
            }


        }
    }
}