using API_Inteleco.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TornfyApp.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TornfyApp.ViewModel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetalheEtapaPerformance : ContentPage
    {
        public DetalheEtapaPerformance(Stats conteudo)
        {

            InitializeComponent();
            if (conteudo != null)
            {
                CarregarInfo(conteudo);
                nome_etapa.Text = conteudo.Nome_Etapa;
            }

        }


        public void CarregarInfo(Stats conteudo)
        {
            Clube item1 = new Clube
            {
                item = conteudo.ano.ToString(),
                item_index = 1,
                texto_botao = "Temporada"
            };
            Clube item2 = new Clube
            {
                item = conteudo.total_etapa.ToString(),
                item_index = 2,
                texto_botao = "Qtd Total"
            };
            Clube item3 = new Clube
            {
                item = conteudo.total_cat.ToString(),
                item_index = 3,
                texto_botao = "Qtd Categoria"
            };

            Clube item4 = new Clube
            {
                item = conteudo.mp,
                item_index = 1,
                texto_botao = "Melhor Posição"
            };
            Clube item5 = new Clube
            {
                item = conteudo.SomaSetGanhos.ToString(),
                item_index = 2,
                texto_botao = "Set's"
            };
            Clube item6 = new Clube
            {
                item = conteudo.SomaGamesGanhos.ToString(),
                item_index = 3,
                texto_botao = "Games"
            };

            Clube item7 = new Clube
            {
                item = conteudo.vit_etapa.ToString(),
                item_index = 1,
                texto_botao = "Vit/Der"
            };
            Clube item8 = new Clube
            {
                item = conteudo.performance.ToString(),
                item_index = 2,
                texto_botao = "Performance"
            };



            List<Clube> Lista_Clube = new List<Clube>();
            Lista_Clube.Add(item1);
            Lista_Clube.Add(item2);
            Lista_Clube.Add(item3);
            Lista_Clube.Add(item4);
            Lista_Clube.Add(item5);
            Lista_Clube.Add(item6);
            Lista_Clube.Add(item7);
            Lista_Clube.Add(item8);


            listagem_clubes.ItemsSource = Lista_Clube.ToList();
        }

        private void listagem_clubes_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private void listagem_clubes_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
    }
}