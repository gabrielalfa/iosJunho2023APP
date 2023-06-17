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
    public partial class PotuacaoParcial : ContentPage
    {

        public List<Ranking> lista_pontos;

        public PotuacaoParcial()
        {
            InitializeComponent();
        }

        public PotuacaoParcial(int id_jogador, int ano, int id_categoria, string Nome_Categoria, string Nome_jogador, string ball)
        {
            InitializeComponent();
            //txt_top.Text = string.Format("Categoria: {0}", Nome_Categoria);
            //txt_nome.Text = string.Format("{0} ({1})", Nome_jogador, ano);
            CarregarPontuacao_Parcial(id_jogador, id_categoria, ano);

            Title = Nome_jogador.ToString() + "/" + Nome_Categoria.ToString();
            txt_top.Text = Nome_jogador.ToString();
            txt_nome.Text = Nome_Categoria.ToString();
            img_ball.Source = ball;
        }

        public async void CarregarPontuacao_Parcial(int id_jogador, int id_categoria, int ano)
        {
            lista_pontos = await API_Service.ObterPontuacaoParcial(id_jogador, id_categoria, ano);
            lst_ponrutacaoParcial.ItemsSource = lista_pontos.ToList();
        }

        private void lst_ponrutacaoParcial_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private void lst_ponrutacaoParcial_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
    }
}