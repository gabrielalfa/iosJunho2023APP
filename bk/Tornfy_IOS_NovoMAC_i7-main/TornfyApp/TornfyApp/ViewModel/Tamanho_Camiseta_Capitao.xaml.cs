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
    public partial class Tamanho_Camiseta_Capitao : ContentPage
    {
        int id_etapa = 0;
        int id_jogador = 0;
        int id_dupla = 0;
        string nome_etapa = "";
        string nome_categoria = "";
        string nome_parceiro = "";
        int id_categoria = 0;
        public List<Etapa> Lista_camisetas;
        public string nome_jogador;
        public int id_tipo;
        public int id_master;

        public Tamanho_Camiseta_Capitao(int id_master, int Id_etapa, int Id_jogador,
            string Nome_etapa, int Id_categoria, string Nome_categoria, int Id_dupla, string Nome_parceiro, string _nome_jogador, int _id_tipo)
        {
            InitializeComponent();

            id_etapa = Id_etapa;
            id_jogador = Id_jogador;
            id_dupla = Id_dupla;
            nome_etapa = Nome_etapa;
            id_categoria = Id_categoria;
            nome_categoria = Nome_categoria;
            nome_parceiro = Nome_parceiro;

            txt_nome_etapa.Text = nome_etapa;
            txt_nome_categoria.Text = nome_categoria;
            txt_nome_parceiro.Text = Nome_parceiro;
            this.id_master = id_master;
            nome_jogador = _nome_jogador.ToString();

            CarregarInfoEtapa();

        }

        public async void CarregarInfoEtapa()
        {
            Lista_camisetas = await API_Service.ObterTamanhosCamiseta();
            lista_camisetas.ItemsSource = Lista_camisetas.ToList();

        }

        private void lista_camisetas_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private async void lista_camisetas_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Etapa etapa = ((ListView)sender).SelectedItem as Etapa;
            if (etapa == null)
            {
                return;
            }

            await Navigation.PushAsync(new Tamanho_Camiseta_Parceiro(id_master, id_etapa, id_jogador,
               nome_etapa, id_categoria, nome_categoria, id_dupla, etapa.tamanho, nome_parceiro, nome_jogador, id_tipo));
        }
    }
}
