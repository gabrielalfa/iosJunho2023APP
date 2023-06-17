using System;
using TornfyApp.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TornfyApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage_oficial(false, 0, "", ""));
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
