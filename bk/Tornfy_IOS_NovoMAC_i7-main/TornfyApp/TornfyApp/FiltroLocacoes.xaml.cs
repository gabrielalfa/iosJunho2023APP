using Acr.UserDialogs.Extended;
using TornfyApp.Model;
using ICT.ViewModel;
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
    public partial class FiltroLocacoes : PopupPage
    {
        public Config_Home config;
        public int id_master;
        public int id_jogador;

        public FiltroLocacoes(Config_Home _config, int id_master, int id_jogador)
        {
            InitializeComponent();

            this.id_master = id_master;
            this.id_jogador = id_jogador;
            config = _config;
        }

        private async void btn_close_Clicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading(title: "Buscando Etapas");
            int index = pcStatus.SelectedIndex;
            if (index == -1) index = 0;
            var itemSelecionado = pcStatus.Items[index];
            int status = 0;
            switch (itemSelecionado)
            {
                case "Pagas":
                    status = 1;
                    break;
                case "Não Pagas":
                    status = 2;
                    break;
                default:
                    status = 0;
                    break;
            }

            //await Navigation.PushAsync(new Historico_Locacoes(config, id_master, id_jogador, startDatePicker.Date.ToString(), endDatePicker.Date.ToString(), status));
            UserDialogs.Instance.HideLoading();

            _ = App.Current.MainPage.Navigation.PopPopupAsync(true);
        }

        void OnDateSelected(object sender, DateChangedEventArgs args)
        {
            Recalculate();
        }

        void OnSwitchToggled(object sender, ToggledEventArgs args)
        {
            Recalculate();
        }

        void Recalculate()
        {
            bool includeSwitch = true;
            TimeSpan timeSpan = endDatePicker.Date - startDatePicker.Date +
                (includeSwitch ? TimeSpan.FromDays(1) : TimeSpan.Zero);

            resultLabel.Text = String.Format("{0} dias{1} entre as datas",
                                                timeSpan.Days, timeSpan.Days == 1 ? "" : "s");
        }
    }
}