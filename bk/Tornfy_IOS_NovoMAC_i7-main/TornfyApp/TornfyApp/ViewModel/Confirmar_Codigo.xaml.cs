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
    public partial class Confirmar_Codigo : ContentPage
    {
        public string codigo;
        public int id_jogador;
        public List<Temporada> lista_circuitos;

        public Confirmar_Codigo(string _codigo, int id_jogador)
        {
            codigo = _codigo;
            InitializeComponent();
            this.id_jogador = id_jogador;
        }

        private async void btn_confirmar_Clicked(object sender, EventArgs e)
        {
            if (codigo == txtCodigo.Text.Trim())
            {
                //liberar o login
                int result = await API_Service.Confirmar_Codigo_post(id_jogador, int.Parse(txtCodigo.Text.Trim()));

                if (result == 200)
                {
                    var viewPage = new LoginPage_oficial(true, id_jogador, "", "");
                    var navigation = Application.Current.MainPage as NavigationPage;
                    await navigation.PushAsync(viewPage, true);
                }

            }
            else
            {
                var pop = new MessageBox("Código Incorreto", "O código não confere com o enviado ao seu email!");
                await App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var viewPage = new LoginPage_oficial(false, 0, "", "");
            var navigation = Application.Current.MainPage as NavigationPage;
            navigation.PushAsync(viewPage, true);
        }
    }
}