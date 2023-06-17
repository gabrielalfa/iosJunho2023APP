using ICT.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace TornfyApp.Model
{
    public class RegistroViewModel
    {


        public ICommand CancelCommand => new Command(ReturnLoucher);

        private void ReturnLoucher()
        {
            //var viewPage = new Login_2();
            //var navigation = Application.Current.MainPage as NavigationPage;
            //navigation.PushAsync(viewPage, true);
        }

    }
}
