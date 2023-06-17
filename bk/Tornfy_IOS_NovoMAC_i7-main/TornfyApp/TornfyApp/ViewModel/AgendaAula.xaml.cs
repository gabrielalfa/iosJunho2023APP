	using System;
using System.Collections.Generic;
using Acr.UserDialogs.Extended;
using TornfyApp.API;
using TornfyApp.Model;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TornfyApp.ViewModel
{
    public partial class AgendaAula : ContentPage
    {
        public Config_Home config;
        public int id_jogador;
        public int id_agenda;

        public AgendaAula(Config_Home config, int id_jogador)
        {
            this.config = config;
            this.id_jogador = id_jogador;

            InitializeComponent();
        }

        public async void CarregarAgendas()
        {
            UserDialogs.Instance.ShowLoading(title: "Buscando Agendas");
            List<Agenda> Lista_torneios = await API_Service.Obter_Agendas(id_jogador);
            listagem_agendas.ItemsSource = Lista_torneios;
            UserDialogs.Instance.HideLoading();
        }



        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (TestarInternet()) { CarregarAgendas(); } else { Navigation.PushAsync(new NoInternet()); }
        }

        public static bool TestarInternet()
        {
            bool coneccao = true;
            if (Connectivity.NetworkAccess != NetworkAccess.Internet) { coneccao = false; }
            return coneccao;
        }

        private void listagem_agendas_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private async void listagem_agendas_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Agenda agenda = ((ListView)sender).SelectedItem as Agenda;
            if (agenda == null)
            {
                return;
            }

            await Navigation.PushAsync(new Agenda_Compartilhada(config, id_jogador, agenda.id_master));

        }

        private void wolcome_name_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Dados_Jogador(config, id_jogador, 1));
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading(title: "Buscando Etapas");
            await Navigation.PushAsync(new Etapas_Recentes(config, id_jogador, 0, null, null, null));
            UserDialogs.Instance.HideLoading();
        }
    }
}

