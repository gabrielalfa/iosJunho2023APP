using Acr.UserDialogs.Extended;
using Rg.Plugins.Popup.Extensions;
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
    public partial class Creditos : ContentPage
    {
        public int id_jogador;
        public int id_master;
        List<Inscricao> Lista_Creditos;

        List<Credito> Lista_Creditos_locacao;

        public Creditos(int id_jogador, int id_master)
        {
            InitializeComponent();

            this.id_master = id_master;
            this.id_jogador = id_jogador;
            list_principal.IsVisible = true;
            //stk_semregistro.IsVisible = false;
        }

        protected override void OnAppearing()
        {
            UserDialogs.Instance.ShowLoading(title: "Buscando Lista");
            base.OnAppearing();
            CarregarInscricoes();
            CarregarCreditoLocacao();


            UserDialogs.Instance.HideLoading();
        }

        public async void CarregarCreditoLocacao()
        {
            Lista_Creditos_locacao = await API_Service.ObterLista_CreditosLocacao(id_jogador);
            listacreditolocacao.ItemsSource = Lista_Creditos_locacao.ToList();

        }
        public async void CarregarInscricoes()
        {

            Lista_Creditos = await API_Service.ObterLista_Creditos(id_jogador);

            if (Lista_Creditos.Count != 0)
            {
                foreach (var item in Lista_Creditos)
                {
                    if (string.IsNullOrEmpty(item.responsavel)) item.responsavel = "não identificado";
                }

                etapas.ItemsSource = Lista_Creditos.ToList();

            }

            etapas.ItemsSource = Lista_Creditos.ToList();

        }

        private void etapas_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private void etapas_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            if (((ListView)sender).SelectedItem as Inscricao == null)
            {
                return;
            }
            else
            {
                var pop2 = new MensagemComConfirmacao("Enviar Mensagem!",
                    string.Format("Deseja enviar mensagem e entrar em contato com o organizador: {0} pelo contato: {1}.",
                    (((ListView)sender).SelectedItem as Inscricao).responsavel, (((ListView)sender).SelectedItem as Inscricao).telefone),
                    "#128c7e", "Enviar Mensagem?", true, (((ListView)sender).SelectedItem as Inscricao).telefone, false, "", 0);
                _ = App.Current.MainPage.Navigation.PushPopupAsync(pop2, true);
            }

        }
    }
}