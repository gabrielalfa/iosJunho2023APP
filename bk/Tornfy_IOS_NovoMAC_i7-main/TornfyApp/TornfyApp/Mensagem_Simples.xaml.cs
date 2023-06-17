using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TornfyApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Mensagem_Simples : PopupPage
    {
        public Mensagem_Simples(string titulo, string mensagem, string color_btn, string text_btn)
        {
            InitializeComponent();

            txt_titulo.Text = titulo;
            txt_mensagem.Text = mensagem;
            btn_close.BackgroundColor = Color.FromHex(color_btn.ToString());
            btn_close.Text = text_btn;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage.Navigation.PopPopupAsync(true);
        }
    }
}