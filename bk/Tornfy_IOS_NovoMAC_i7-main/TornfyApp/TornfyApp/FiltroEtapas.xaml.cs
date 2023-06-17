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
using TornfyApp.ViewModel;
using System.Globalization;

namespace TornfyApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FiltroEtapas : PopupPage
    {
        public static string Info;
        public int id_jogador;
        public int local;
        public int id_grupo;
        public Config_Home config;
        public int tipo_etapa_sel;

        public string path;
        public string Nome_Ranking;

        public FiltroEtapas(Config_Home _config, int _id_jogador, int _tipo_etapa_sel, int local, int id_grupo,  string path, string Nome_Ranking)
        {
            config = _config;
            id_jogador = _id_jogador;
            this.local = local;
            this.id_grupo = id_grupo;
            tipo_etapa_sel = _tipo_etapa_sel;

            this.path = path;
            this.Nome_Ranking = Nome_Ranking;

            InitializeComponent();
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            bool bool_situacao = includeSwitch.IsToggled;
            UserDialogs.Instance.ShowLoading(title: "Buscando Etapas");
            if (local == 1) {  await Navigation.PushAsync(new Etapas_Recentes(config, id_jogador, tipo_etapa_sel, startDatePicker.Date.ToString("dd/MM/yyyy", new CultureInfo("pt-BR")), endDatePicker.Date.ToString("dd/MM/yyyy", new CultureInfo("pt-BR")), bool_situacao.ToString())); }
            if (local == 2) { await Navigation.PushAsync(new ListaTorneiosModalidade(id_jogador, tipo_etapa_sel, startDatePicker.Date.ToString("dd/MM/yyyy", new CultureInfo("pt-BR")), endDatePicker.Date.ToString("dd/MM/yyyy", new CultureInfo("pt-BR")), bool_situacao.ToString())); }

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
            TimeSpan timeSpan = endDatePicker.Date - startDatePicker.Date +
                (includeSwitch.IsToggled ? TimeSpan.FromDays(1) : TimeSpan.Zero);

            resultLabel.Text = String.Format("{0} dias{1} entre as datas",
                                                timeSpan.Days, timeSpan.Days == 1 ? "" : "s");
        }

        private void pcCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}