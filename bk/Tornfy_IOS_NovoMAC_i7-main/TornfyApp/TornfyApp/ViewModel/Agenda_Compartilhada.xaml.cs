using System;
using System.Collections.Generic;
using Acr.UserDialogs.Extended;
using TornfyApp.Model;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TornfyApp.ViewModel
{
    public partial class Agenda_Compartilhada : ContentPage
    {

        public Config_Home config;
        public int id_jogador;
        public int id_agenda;

        public Agenda_Compartilhada(Config_Home config, int id_jogador, int id_agenda)
        {

            this.config = config;
            this.id_jogador = id_jogador;
            this.id_agenda = id_agenda;

            InitializeComponent();

            string url = string.Format("https://tornfy.com/Agenda_Compartilhada_WebView?id_master={0}&id_jogador={1}", id_agenda, id_jogador);
            webView.Source = url;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!TestarInternet()) { Navigation.PushAsync(new NoInternet()); } 
        }

        public static bool TestarInternet()
        {
            bool coneccao = true;
            if (Connectivity.NetworkAccess != NetworkAccess.Internet) { coneccao = false; }
            return coneccao;
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

