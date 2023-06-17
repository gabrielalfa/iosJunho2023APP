using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Extensions;
using System.Linq;
using TornfyApp.API;
using TornfyApp.Model;
using Xamarin.Forms;

namespace TornfyApp.ViewModel
{
    public partial class Outros_Complexos : ContentPage
    {
        public int id_jogador;
        public string telefone;
        public string email;
        public string nome_jogador;
        public bool clicado = false;

        List<Quadra> lista_locais;
        List<Quadra> lista_locais_outros;
        public Config_Home config;

        public Outros_Complexos(int id_jogador, string telefone, string email, string nome_jogador)
        {
            InitializeComponent();

            this.id_jogador = id_jogador;
            this.telefone = telefone;
            this.email = email;
            this.nome_jogador = nome_jogador;
            CarregarComplexos_outros();

        }

        public async void CarregarComplexos_outros()
        {
            lista_locais_outros = await API_Service.ObterLocaisLocacao_outros(id_jogador);
            listagem_complexos.ItemsSource = lista_locais_outros.ToList();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (clicado)
            {
                DisplayAlert("Aviso de Sistema!", "Fique tranquilo, " +
                   string.Format("enviamos um email para o organizador avisando " +
                   "da sua habilitação, aguarde o retorno em breve. Se necessário entre em contato: {0}", telefone), "fechar");
            }

        }

        private void listagem_complexos_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private void listagem_complexos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Quadra complexo = ((ListView)sender).SelectedItem as Quadra;
            if (complexo == null) { return; }

            clicado = true;

            string mensagem = string.Format("Olá, meu nome é {2}, sou um jogador cadastrado na Plataforma tornfy.com com o ID: {3} " +
                              "e gostaria de liberar meu acesso as suas locações de quadra. " +
                              "Meu email de cadastro é:  {1} e meu telefone: {0}", this.telefone, this.email, this.nome_jogador, id_jogador);

            var pop2 = new MensagemComConfirmacao("Enviar Mensagem!", mensagem, "#128c7e", "Enviar Solicitação?",
                true, complexo.telefone, true, complexo.email, complexo.id);
            _ = App.Current.MainPage.Navigation.PushPopupAsync(pop2, true);
        }
    }
}

