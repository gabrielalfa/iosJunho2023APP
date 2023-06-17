using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace TornfyApp.ViewModel
{	
	public partial class ErrorPage : ContentPage
	{	
		public ErrorPage ()
		{
			InitializeComponent ();
		}

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CentralAjuda());
        }
    }
}

