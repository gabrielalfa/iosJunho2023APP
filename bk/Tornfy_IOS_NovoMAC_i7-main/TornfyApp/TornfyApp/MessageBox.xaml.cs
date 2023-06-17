using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TornfyApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessageBox : PopupPage
    {

        public MessageBox(string titulo, string mensagem)
        {
            InitializeComponent();

            txt_titulo.Text = titulo;
            txt_mensagem.Text = mensagem;
        
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage.Navigation.PopPopupAsync(true);
        }
    }
}