using Acr.UserDialogs.Extended;
using API_Inteleco.Models;
using System;
using System.Collections.Generic;
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
    public partial class Editar_DadosJogador : ContentPage
    {
        int id_jogador = 0;
        public Jogador info_jogador;
        public List<Clube> lista_clubes;
        int id_master = 0;


        public Editar_DadosJogador(int Id_jogador, int master)
        {
            id_jogador = Id_jogador;
            id_master = master;

            InitializeComponent();
            CarregarDadosJogador(id_jogador);

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            CarregarDadosJogador(id_jogador);


        }

        public async void CarregarDadosJogador(int id_jogador)
        {
            info_jogador = await API_Service.ObterDadosJogador(id_jogador);

            txt_nomeJogador.Text = info_jogador.Nome_Jogador;

            txt_email.Text = info_jogador.Email;
            txt_telfone.Text = info_jogador.telefone;
            txt_password.Text = info_jogador.password;

            //_menuLista = ItemService.GetDadosJogador(info_jogador);
            //dados_jogador.ItemsSource = _menuLista;

        }


        public async void CarregarClubes()
        {

            UserDialogs.Instance.ShowLoading(title: "Buscando Clubes");

            lista_clubes = await API_Service.ObterClubes(id_master, null);
            //pk_tenis.ItemsSource = lista_clubes.ToList();
            //pk_beach.ItemsSource = lista_clubes.ToList();

            UserDialogs.Instance.HideLoading();



        }

        private async void btnIncluirImagem_Clicked(object sender, EventArgs e)
        {
            Jogador dados = new Jogador();
            dados.Email = txt_email.Text;
            dados.telefone = txt_telfone.Text;
            dados.password = txt_password.Text;
            dados.id_jogador = id_jogador;


            int result = await API_Service.EditarJogador(dados);

            if (result == 500)
            {
                await DisplayAlert("Edição não realizada", "Falha ao editar jogador!!!", "Fechar");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Edição realizada", "Jogador Editado com sucesso!!!", "Fechar");
                await Navigation.PopAsync();
            }

        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}