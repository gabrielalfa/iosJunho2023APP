using Acr.UserDialogs.Extended;
using API_Inteleco.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TornfyApp.API;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TornfyApp.ViewModel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Clubes : ContentPage
    {

        public List<Clube> Lista_Clubes;

        public Task<ObservableCollection<Clube>> etapas;

        public int id_master;

        public Clubes(int _id_master)
        {
            InitializeComponent();
            id_master = _id_master;
            CarregarClubes(id_master, "vazio");

        }

        public async void CarregarClubes(int id_master, string busca)
        {
            UserDialogs.Instance.ShowLoading(title: "Buscando Lista");
            Lista_Clubes = await API_Service.ObterClubes_qtd(id_master, busca);
            UserDialogs.Instance.HideLoading();

            this.listagem_clubes.ItemsSource = this.Lista_Clubes.ToList();
            
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {

        }

        private void txt_busca_SearchButtonPressed(object sender, EventArgs e)
        {
            CarregarClubes(id_master, txt_busca.Text);
        }

        private void listagem_clubes_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private void listagem_clubes_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var itens = this.Lista_Clubes as IList;
            var itens2 = this.listagem_clubes as IList;

            if (API_Service.qtd_chamada <= itens.Count)
            {
                if (itens != null && e.Item == itens[itens.Count - 1])
                {
                    CarregarClubes(id_master, txt_busca.Text);
                }
            }

        }

        private async void listagem_clubes_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Clube clube = ((ListView)sender).SelectedItem as Clube;
            if (clube == null)
            {
                return;
            }

            UserDialogs.Instance.ShowLoading(title: "Buscando Informações");
            await Navigation.PushAsync(new Clubes_Contato(clube.email, clube.Telefone, clube.Responsavel));
            UserDialogs.Instance.HideLoading();
        }
    }
}