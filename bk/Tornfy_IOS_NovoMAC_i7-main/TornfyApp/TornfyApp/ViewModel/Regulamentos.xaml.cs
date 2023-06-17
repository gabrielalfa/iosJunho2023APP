using Acr.UserDialogs.Extended;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TornfyApp.API;
using TornfyApp.Model;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TornfyApp.ViewModel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Regulamentos : ContentPage
    {
        List<Temporada> lista_circuitos;

        public Regulamentos()
        {
            InitializeComponent();
            CarregarCircuitos();

            txt_subtitulo.Text = "Circuitos Temporadas 2022";

        }


        public async void CarregarCircuitos()
        {
            UserDialogs.Instance.ShowLoading(title: "Buscando Lista");
            lista_circuitos = await API_Service.ObterCircuitos_Menu_all();
            UserDialogs.Instance.HideLoading();

            listagem_torneios.ItemsSource = lista_circuitos.ToList();

        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {

        }

        private void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {

        }

        private void listagem_torneios_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private async void listagem_torneios_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            Temporada ranking = ((ListView)sender).SelectedItem as Temporada;
            if (ranking == null)
            {
                return;
            }

            try
            {
                var filePath = await DownloadPdfFileAsyncProgramacao(ranking.id_ranking);

                if (filePath != null)
                {
                    await Launcher.OpenAsync(new OpenFileRequest
                    {
                        File = new ReadOnlyFile(filePath)
                    });
                }
            }
            catch (Exception)
            {
                UserDialogs.Instance.HideLoading();
                var pop = new MessageBox("Não Publicado!", "Este regulamento ainda não foi publicado pelo gestor de circuito!");
                await Application.Current.MainPage.Navigation.PushPopupAsync(pop, true).ConfigureAwait(false);
            }


        }


        private async Task<string> DownloadPdfFileAsyncProgramacao(int id)
        {


            UserDialogs.Instance.ShowLoading(title: "Buscando Jogos");

            string url = "";

            var filePath = Path.Combine(FileSystem.AppDataDirectory, "Regulamento_" + id + ".pdf");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                //return filePath;
            }

            url = string.Format("https://ctsg.com.br/Regulamento/GetPdf/{0}", id);

            var httpClient = new HttpClient();
            var pdfBytes = await httpClient.GetByteArrayAsync(url);

            try
            {
                File.WriteAllBytes(filePath, pdfBytes);
                UserDialogs.Instance.HideLoading();
                return filePath;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
            }

            UserDialogs.Instance.HideLoading();
            return null;
        }
    }
}