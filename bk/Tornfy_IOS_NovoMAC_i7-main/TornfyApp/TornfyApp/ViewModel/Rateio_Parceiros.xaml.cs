using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TornfyApp.API;
using TornfyApp.Model;
using Xamarin.Forms;

namespace TornfyApp.ViewModel
{
    public partial class Rateio_Parceiros : ContentPage
    {

        public int id_jogador;
        public int id_master;
        public bool recentes;
        public string busca;
        public List<Quadra> Lista_parceiros;
        public List<Jogador> Lista_parceiros_quadra;
        Quadra quadra = new Quadra();

        public Rateio_Parceiros(int id_jogador, int id_master, bool recentes, string busca, List<Quadra> lista_rateio)
        {
            InitializeComponent();

            this.id_jogador = id_jogador;
            this.id_master = id_master;
            this.recentes = recentes;
            this.busca = busca; Buscar_Parceiros(lista_rateio);
        }

        public async void Buscar_Parceiros(List<Quadra> lista_rateio)
        {
            Lista_parceiros = await API_Service.ObterParceiros(id_jogador, id_master, recentes, busca, lista_rateio);

            foreach (var item in Lista_parceiros)
            {
                if (!string.IsNullOrEmpty(item.path))
                {
                    HttpWebResponse response = null;
                    var request = (HttpWebRequest)WebRequest.Create(item.path);
                    request.Method = "HEAD";
                    try
                    {
                        response = (HttpWebResponse)request.GetResponse();
                        item.path = item.path;
                    }
                    catch (WebException ex)
                    {
                        item.path = "user_round.png";
                    }
                    finally { if (response != null) { response.Close(); } }
                }
                else { item.path = "user_round.png"; }
            }



            lista_pagamentos.ItemsSource = Lista_parceiros.ToList();
        }

        private void txtSearch_SearchButtonPressed(object sender, EventArgs e)
        {

        }

        private void lista_pagamentos_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        public event EventHandler<Quadra> ItemSelecionado;

        private async void lista_pagamentos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedItem = (Quadra)((ListView)sender).SelectedItem;

            if (selectedItem != null)
            {
                ItemSelecionado?.Invoke(this, selectedItem);
                await Navigation.PopAsync();
            }
        }

        private async void  ImageButton_Clicked(System.Object sender, System.EventArgs e)
        {
            // Obtém o botão de imagem clicado
            var imageButton = (ImageButton)sender;

            // Obtém o item associado ao botão de imagem
            var selectedItem = (Quadra)imageButton.BindingContext;

            if (selectedItem != null)
            {
                ItemSelecionado?.Invoke(this, selectedItem);
                await Navigation.PopAsync();
            }
        }
    }
}

