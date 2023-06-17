using System;
using System.Collections.Generic;
using TornfyApp.Model;
using Xamarin.Forms;

namespace TornfyApp.ViewModel
{	
	public partial class Planos : ContentPage
	{
        public Config_Home config;
        public int id_jogador;
        public int id_agenda;

        public Planos(Config_Home config, int id_jogador)
		{
			InitializeComponent ();
		}

        void listagem_agendas_ItemTapped(System.Object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        void listagem_agendas_ItemSelected(System.Object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
        }
    }
}

