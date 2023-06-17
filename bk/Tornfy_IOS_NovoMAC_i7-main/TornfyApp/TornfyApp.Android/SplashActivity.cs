using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TornfyApp.Droid;

namespace ICT.Droid
{
    [Activity(Label = "Tornfy", Icon = "@mipmap/launcher_foreground", Theme = "@style/SplashTheme",
        MainLauncher = true, NoHistory = true)]
    class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            //Thread.Sleep(2000);
            //Finish();
        }

        protected override async void OnResume()
        {
            base.OnResume();
            await SimulateStartup();
        }

        async Task SimulateStartup()
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}