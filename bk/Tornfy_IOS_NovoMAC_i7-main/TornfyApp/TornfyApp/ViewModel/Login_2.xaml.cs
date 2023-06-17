using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TornfyApp.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TornfyApp.ViewModel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login_2 : ContentPage
    {
        public List<Temporada> lista_circuitos;
        public Login_2()
        {
            InitializeComponent();

        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var viewPage = new ConfirmarCPF();
            var navigation = Application.Current.MainPage as NavigationPage;
            navigation.PushAsync(viewPage, true);
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            var viewPage = new LoginPage_oficial(false, 0, "", "");
            var navigation = Application.Current.MainPage as NavigationPage;
            navigation.PushAsync(viewPage, true);

        }


    }
}