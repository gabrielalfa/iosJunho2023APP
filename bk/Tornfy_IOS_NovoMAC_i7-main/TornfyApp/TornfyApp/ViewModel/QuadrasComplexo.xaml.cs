using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TornfyApp.API;
using TornfyApp.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


using Xamarin.Forms;
using Acr.UserDialogs.Extended;
using System;

namespace TornfyApp.ViewModel
{
    public partial class QuadrasComplexo : ContentPage
    {

        public Config_Home config;
        public string data { get; set; }
        public int id_master;
        public List<Quadra> Lista_quadras;
        public List<Quadra> Lista_horarios;
        public int id_quadra_selecionada;
        public int id_jogador;
        public string data_sel;
        public string nome_locacoes;
        public int id_proprietario;


        public List<Quadra> Lista_pagamentos;



        public QuadrasComplexo(Config_Home Config, int id_master, int id_jogador, string nome_locacoes, int id_proprietario)
        {
            InitializeComponent();

            ToolbarItems.Remove(cart_shop_ball);
            ToolbarItems.Remove(cart_shop);

            config = Config;
            this.id_master = id_master;
            this.id_jogador = id_jogador;
            this.nome_locacoes = nome_locacoes;
            this.id_proprietario = id_proprietario;

            Title = nome_locacoes;
        }

        public async void CarregarQuadras()
        {
            Lista_quadras = await API_Service.ObterQuadras(id_master);





            if (Lista_quadras != null)
            {
                foreach (var item in Lista_quadras)
                {
                    item.string_quadra = "Quadra " + "(" + item.Nome_Quadra + ")";
                }
                lista_quadras.ItemsSource = Lista_quadras.ToList();
            }

        }

        public async void Buscar_Pagamentos()
        {
            Lista_pagamentos = await API_Service.ObterPagamentosQuadra("", "", id_master, id_jogador);

            bool pagamento = false;
            foreach (var item in Lista_pagamentos)
            {
                if (!item.status_pagamento)
                {
                    pagamento = true;
                    break;
                }
            }

            if (pagamento) { ToolbarItems.Add(cart_shop_ball); } else { ToolbarItems.Add(cart_shop); }

            //carroucel_horarios.ItemsSource = Lista_horarios.ToList();
        }

        protected override void OnAppearing()
        {
            ToolbarItems.Remove(cart_shop_ball);
            ToolbarItems.Remove(cart_shop);

            base.OnAppearing();
            CarregarQuadras();
            Buscar_Pagamentos();

        }

        private void lista_quadras_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private async void lista_quadras_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Quadra local = ((ListView)sender).SelectedItem as Quadra;
            if (local == null)
            {
                return;
            }
            UserDialogs.Instance.ShowLoading(title: "Buscando Horários");
            await Navigation.PushAsync(new Locacoes(config, id_master, id_jogador, local.id, local.string_quadra, nome_locacoes, id_proprietario));
            UserDialogs.Instance.HideLoading();
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Historico_Locacoes(id_master, id_jogador));
        }

        private async void cart_shop_Clicked(System.Object sender, System.EventArgs e)
        {

            await Navigation.PushAsync(new Fechamento_PagamentoLocacao(config, id_jogador, id_master, "", false, 0, 0, false, 0));
        }
    }
}

