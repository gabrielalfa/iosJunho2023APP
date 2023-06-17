using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TornfyApp.ViewModel
{
    public partial class ConfigPage : ContentPage
    {
        public ConfigPage()
        {
            InitializeComponent();



            CarregarStorageConfig();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            // Salva as configurações
            bool receiveNotifications = ((Switch)Content.FindByName("notificationsSwitch")).IsToggled;
            List<string> selectedSports = new List<string>();
            foreach (CheckBox checkbox in Content.FindByName<StackLayout>("sportsStack").Children)
            {
                if (checkbox.IsChecked)
                {
                    //selectedSports.Add(((Label)checkbox.Content).Text);
                }
            }

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CarregarStorageConfig();
        }

        public async void CarregarStorageConfig()
        {
            string notificacao = await Xamarin.Essentials.SecureStorage.GetAsync("notificacao");

            if (string.IsNullOrEmpty(notificacao))
            {
                await Xamarin.Essentials.SecureStorage.SetAsync("notificacao", "no");
            }

            notificationsSwitch.IsToggled = notificacao != "no";

            await CarregarCheckBoxConfig("tenis_ck", ck_tenis);
            await CarregarCheckBoxConfig("beach_ck", ck_beach);
            await CarregarCheckBoxConfig("padel_ck", ck_padel);
            await CarregarCheckBoxConfig("squash_ck", ck_squash);

        }

        private async Task CarregarCheckBoxConfig(string chave, CheckBox checkBox)
        {
            string valor = await Xamarin.Essentials.SecureStorage.GetAsync(chave);

            if (string.IsNullOrEmpty(valor))
            {
                valor = "yes";
                await Xamarin.Essentials.SecureStorage.SetAsync(chave, valor);
            }

            checkBox.IsChecked = valor != "no";
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void Button_Clicked_2(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void notificationsSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            bool isToggled = e.Value;

            if (isToggled)
            {
                await Xamarin.Essentials.SecureStorage.SetAsync("notificacao", "yes");
            }
            else
            {
                await Xamarin.Essentials.SecureStorage.SetAsync("notificacao", "no");
            }
        }

        private async void ck_tenis_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            bool isChecked = e.Value;
            var valor = "no";
            if (isChecked) { valor = "yes"; }
            CheckBox checkBox = sender as CheckBox;
            string chave = checkBox.ClassId.ToString();

            await Xamarin.Essentials.SecureStorage.SetAsync(chave, valor);

        }
    }
}

