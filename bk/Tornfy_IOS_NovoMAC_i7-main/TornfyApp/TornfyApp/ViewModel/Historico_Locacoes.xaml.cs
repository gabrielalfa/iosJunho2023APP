using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace TornfyApp.ViewModel
{	
	public partial class Historico_Locacoes : ContentPage
    {
        public Historico_Locacoes(int id_master, int id_jogador)
        {
            InitializeComponent();

            string url = string.Format("https://tornfy.com/Quadras/Locacoes_Usuario_WebView?id_master={0}&id_jogador={1}", id_master, id_jogador);
            webView.Source = url;
        }
    }
}

	