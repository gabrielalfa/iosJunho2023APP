using API_Inteleco.Models;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TornfyApp.ViewModel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Clubes_Contato : ContentPage
    {

        public string email;
        public string telefone;
        public string responsavel;

        public Clubes_Contato(string email, string telefone, string responsavel)
        {
            InitializeComponent();
            this.email = email;
            this.telefone = telefone;
            this.responsavel = responsavel;
            CarregarInfo();
        }

        public async void CarregarInfo()
        {
            Clube item1 = new Clube
            {
                item = responsavel,
                item_index = 1,
                texto_botao = "copiar"
            };
            Clube item2 = new Clube
            {
                item = telefone,
                item_index = 2,
                texto_botao = "mensagem"
            };
            Clube item3 = new Clube
            {
                item = email,
                item_index = 3,
                texto_botao = "copiar"
            };

            List<Clube> Lista_Clube = new List<Clube>();
            Lista_Clube.Add(item1);
            Lista_Clube.Add(item2);
            Lista_Clube.Add(item3);

            listagem_clubes.ItemsSource = Lista_Clube.ToList();
        }

        private void listagem_clubes_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private async void listagem_clubes_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Clube clube = ((ListView)sender).SelectedItem as Clube;
            if (clube == null)
            {
                return;
            }
            var pop = new MessageBox("Dado copiado!", "Informação copiada para área de transferência.");
            switch (clube.item_index)
            {
                case 1:
                    await Clipboard.SetTextAsync(clube.item);
                    _ = App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
                    break;
                case 2:


                    var pop2 = new MensagemComConfirmacao("Enviar Mensagem!",
                        string.Format("Deseja enviar mensagem e entrar em contato com o organizador: {0} pelo contato: {1}.",
                        responsavel, telefone), "#128c7e", "Enviar Mensagem?", true, clube.item, false, "", 0);
                    _ = App.Current.MainPage.Navigation.PushPopupAsync(pop2, true);

                    break;
                case 3:
                    await Clipboard.SetTextAsync(clube.item);
                    _ = App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
                    break;
                default:
                    break;
            }

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            Clube clube = ((Button)sender).BindingContext as Clube;
            if (clube == null)
            {
                return;
            }
            var pop = new MessageBox("Dado copiado!", "Informação copiada para área de transferência.");
            switch (clube.item_index)
            {
                case 1:
                    await Clipboard.SetTextAsync(clube.item);
                    _ = App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
                    break;
                case 2:


                    var pop2 = new MensagemComConfirmacao("Enviar Mensagem!",
                        string.Format("Deseja enviar mensagem e entrar em contato com o organizador: {0} pelo contato: {1}.",
                        responsavel, telefone), "#128c7e", "Enviar Mensagem?", true, clube.item, false, "", 0);
                    _ = App.Current.MainPage.Navigation.PushPopupAsync(pop2, true);

                    break;
                case 3:
                    await Clipboard.SetTextAsync(clube.item);
                    _ = App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
                    break;
                default:
                    break;
            }
        }
    }
}