using Acr.UserDialogs.Extended;
using API_Inteleco.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TornfyApp.API;
using TornfyApp.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TornfyApp.ViewModel
{
    public class ObservableGroupCollection<S, T> : ObservableCollection<T>
    {
        private readonly S _key;
        public ObservableGroupCollection(IGrouping<S, T> group)
            : base(group)
        {
            _key = group.Key;
        }
        public S Key
        {
            get { return _key; }
        }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Categorias_Inscricao : ContentPage
    {
        public List<Categoria> Lista_Categorias;
        int id_jogador = 0;

        int id_etapa = 0;
        string nome_etapa = "";
        public Config_Home config;
        public string nome_jogador;
        public int id_tipo;
        public int id_master;

        public List<ObservableGroupCollection<string, Categoria>> DadosAgrupados { get; set; }

        public Categorias_Inscricao(int id_master, int Id_etapa, int id_jogador_inscricao, string Nome_etapa, string _nome_jogador, int _id_tipo)
        {
            InitializeComponent();
            

            this.id_jogador = id_jogador_inscricao;
            this.id_etapa = Id_etapa;
            this.nome_etapa = Nome_etapa;
            this.nome_jogador = _nome_jogador.ToString();
            this.id_tipo = _id_tipo;
            this.id_master = id_master;
            txt_nome_etapa.Text = nome_etapa.ToString();
            txt_nome_jogador.Text = _nome_jogador.ToString();

            CarregarCategoriasEtapa(Id_etapa);
        }

        public async void CarregarCategoriasEtapa(int id_etapa)
        {
            Lista_Categorias = await API_Service.ObterCategoriaEtapaBuild(id_etapa, false);

            DadosAgrupados = Lista_Categorias.OrderBy(p => p.grupo_categoria)
                             .GroupBy(p => p.grupo_categoria.ToString())
                             .Select(p => new ObservableGroupCollection<string, Categoria>(p)).ToList();

            lista_categorias.ItemsSource = DadosAgrupados;

        }

        private void lista_categorias_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private async void lista_categorias_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Categoria categoria = ((ListView)sender).SelectedItem as Categoria;
            if (categoria == null)
            {
                return;
            }

            if (categoria.tipo_simples)
            {
                await Navigation.PushAsync(new Tamanho_Camiseta(id_master, id_etapa, id_jogador, nome_etapa, categoria.id, categoria.Categoria_Nome, nome_jogador, id_tipo));
            }
            else
            {
                UserDialogs.Instance.ShowLoading(title: "Buscando Jogadores");
                await Navigation.PushAsync(new Escolher_Parceiro(id_master, id_etapa, id_jogador, nome_etapa, categoria.id, categoria.Categoria_Nome, nome_jogador, id_tipo));
                UserDialogs.Instance.HideLoading();

            }


        }
    }
}