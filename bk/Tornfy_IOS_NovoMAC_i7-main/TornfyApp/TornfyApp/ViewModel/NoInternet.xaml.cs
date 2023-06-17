using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TornfyApp.ViewModel
{
    public partial class NoInternet : ContentPage
    {
        int count = 1;
        Label label;

        public NoInternet()
        {
            InitializeComponent();
            //TimeLoopPage();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }


        public void TimeLoopPage()
        {
            Device.StartTimer(TimeSpan.FromSeconds(100), () =>
            {
                if (count > 100) return false;

                bool coneccao = true;
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    coneccao = false;
                }

                if (coneccao) Navigation.PopAsync();

                count++;
                return true;
            });

            Content = label;
        }
    }
}

