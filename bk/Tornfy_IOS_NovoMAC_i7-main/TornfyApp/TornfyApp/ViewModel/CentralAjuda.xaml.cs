using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TornfyApp.ViewModel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CentralAjuda : ContentPage
    {
        public CentralAjuda()
        {
            InitializeComponent();
            CarregarFrames();
        }

        public void CarregarFrames()
        {
            Lista_Items item1 = new Lista_Items
            {
                titulo = "Login & Registro",
                texto = "Dificuldades gerais e problemas relacionados a login e registro",
                icone = "faq_lock",
                item_index = 1,
                cor_botao = "#ffd900",

            };
            Lista_Items item2 = new Lista_Items
            {
                titulo = "Pagamentos e Saldos",
                texto = "Dúvidas e respostas sobre pagamentos de eventos realizados na plataforma!",
                icone = "qrcode",
                item_index = 2,
                cor_botao = "#0078ff",
            };
            Lista_Items item3 = new Lista_Items
            {
                titulo = "Suporte ao Jogador",
                texto = "Direcionamos os jogadores ao suporte mais proximo do seu evento",
                icone = "menu_faq",
                item_index = 3,
                cor_botao = "#ced4da",
            };
            Lista_Items item4 = new Lista_Items
            {
                titulo = "Ranking e Pontuação",
                texto = "Correção de pontuação, rankeamento e chaveamento de etapas e circuitos.",
                icone = "menu_ranking",
                item_index = 4,
                cor_botao = "#04c5c5",
            };
            Lista_Items item5 = new Lista_Items
            {
                titulo = "Torneios e Inscrições",
                texto = "Dúvidas frequentes sobre inscrições, categorias e chaves.",
                icone = "menu_edit",
                item_index = 5,
                cor_botao = "#ff9700",
            };
            Lista_Items item6 = new Lista_Items
            {
                titulo = "Relatar uma falha",
                texto = "Se você percebeu uma falha na usabilidade da plataforma, relate para nós.",
                icone = "faq_bug",
                item_index = 6,
                cor_botao = "#ff1b30",
            };



            List<Lista_Items> lsita_performance = new List<Lista_Items>();

            lsita_performance.Add(item1);
            lsita_performance.Add(item2);
            lsita_performance.Add(item3);
            lsita_performance.Add(item4);
            lsita_performance.Add(item5);
            lsita_performance.Add(item6);


            carroucel_days.ItemsSource = lsita_performance.ToList();

        }


        private void carroucel_days_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private async void carroucel_days_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Lista_Items item = ((ListView)sender).SelectedItem as Lista_Items;
            if (item == null)
            {
                return;
            }
            else
            {
                await Navigation.PushAsync(new Faq_perguntas(item.item_index));
            }
        }
    }
}