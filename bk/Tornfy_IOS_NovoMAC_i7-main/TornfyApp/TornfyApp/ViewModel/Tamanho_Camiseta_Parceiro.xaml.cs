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
    public partial class Tamanho_Camiseta_Parceiro : ContentPage
    {
        int id_etapa = 0;
        int id_jogador = 0;
        string nome_etapa = "";
        string nome_categoria = "";
        string tamanho_camisa_capitao = "";
        string nome_parceiro = "";
        int id_dupla = 0;
        int id_categoria = 0;
        public List<Etapa> Lista_camisetas;

        public string nome_jogador;
        public int id_tipo;
        public int id_master;

        public Tamanho_Camiseta_Parceiro(int id_master, int Id_etapa, int Id_jogador,
            string Nome_etapa, int Id_categoria, string Nome_categoria,
            int ID_dupla, string Tamanho_camisa_capitao, string Nome_parceiro, string _nome_jogador, int _id_tipo)
        {
            InitializeComponent();

            id_etapa = Id_etapa;
            id_jogador = Id_jogador;

            nome_etapa = Nome_etapa;
            id_categoria = Id_categoria;
            nome_categoria = Nome_categoria;
            tamanho_camisa_capitao = Tamanho_camisa_capitao;
            id_dupla = ID_dupla;
            nome_parceiro = Nome_parceiro;

            txt_nome_etapa.Text = nome_etapa;
            txt_nome_categoria.Text = nome_categoria;
            txt_nome_parceiro.Text = Nome_parceiro;
            this.id_master = id_master;
            nome_jogador = _nome_jogador.ToString();
            id_tipo = _id_tipo;

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

        private void lista_camisetas_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Etapa etapa = ((ListView)sender).SelectedItem as Etapa;
            if (etapa == null)
            {
                return;
            }

            Navigation.PushAsync(new Confirmar_Inscricao_Dupla(id_master, id_etapa, id_jogador,
                id_categoria, nome_categoria, id_dupla,
                tamanho_camisa_capitao, etapa.tamanho, nome_parceiro, nome_jogador, id_tipo));
        }


    }
}