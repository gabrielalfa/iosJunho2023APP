
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TornfyApp.ViewModel;
using Xamarin.Forms;

namespace TornfyApp.Model
{
    public class ConfirmaCPFViewModel
    {

        public ICommand voltarlogin_command => new Command(voltarlogin);


        private void voltarlogin()
        {
            var viewPage = new LoginPage_oficial(false, 0, "", "");
            var navigation = Application.Current.MainPage as NavigationPage;
            navigation.PushAsync(viewPage, true);
        }

        public ICommand LembrarSenha_Comand => new Command(LembrarSenha);

        private void LembrarSenha()
        {
            var viewPage = new Lembrar_senha();
            var navigation = Application.Current.MainPage as NavigationPage;
            navigation.PushAsync(viewPage, true);
        }


        public ICommand CancelCommand => new Command(ReturnLoucher);

        private void ReturnLoucher()
        {
            var viewPage = new Login_2();
            var navigation = Application.Current.MainPage as NavigationPage;
            navigation.PushAsync(viewPage, true);
        }


    }
}
