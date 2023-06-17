using Acr.UserDialogs.Extended;
using API_Inteleco.Models;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class RelatoriosPage : ContentPage
    {

        public class ObservableGroupCollection<S, T> : ObservableCollection<T>
        {
            private readonly S _key;
            public ObservableGroupCollection(IGrouping<S, T> group)
                : base(group)
            {
                _key = group.Key;
            }
            public S Key
            {
                get { return _key; }
            }
        }

        public List<Categoria> Lista_Categorias;

        int id_etapa = 0;
        public Config_Home config;
        public Etapa info_etapa;
        public int id_tipo;
        public int id_jogador;
        public Jogador info_jogador;
        public string nome_etapa;
        public bool publico;
        public bool anexo;

        public string nome_anexo;
        public string nome_principal;

        public List<ObservableGroupCollection<string, Categoria>> DadosAgrupados { get; set; }

        public RelatoriosPage(int Id_etapa, int id_jogador_inscricao, int _id_tipo,
            string nome_etapa, bool publico, bool anexo,
            string nome_anexo, string nome_principal)
        {
            InitializeComponent();

            stk_semregistro.IsVisible = false;
            lista_categorias.IsVisible = true;
            listagem_pdf.IsVisible = true;

            id_etapa = Id_etapa;
            id_tipo = _id_tipo;
            id_jogador = id_jogador_inscricao;
            this.nome_etapa = nome_etapa;
            this.publico = publico;
            this.anexo = anexo;
            this.nome_anexo = nome_anexo;
            this.nome_principal = nome_principal;

            UserDialogs.Instance.ShowLoading(title: "Buscando Categorias");
            if (TestarInternet()) CarregarChaves_Torneio_Chamada(Id_etapa); else Navigation.PushAsync(new NoInternet());
            if (TestarInternet()) CarregarInfoEtapa();
            UserDialogs.Instance.HideLoading();

        }

        public static bool TestarInternet()
        {
            bool coneccao = true;
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                coneccao = false;
            }
            return coneccao;
        }



        public void CarregarInfoEtapa()
        {

            UserDialogs.Instance.ShowLoading(title: "Buscando Lista");
            try
            {

                Lista_Items item1 = new Lista_Items
                {
                    item = "Programação PDF",
                    item_index = 1,
                    texto_botao = "acessar pdf",
                    cor_botao = "#EC407A",
                    cor_texto = "White",
                    local = nome_principal,
                    icone = "pdf_tab",

                };
                Lista_Items item2 = new Lista_Items
                {
                    item = "Programação Meus Jogos",
                    item_index = 2,
                    texto_botao = "acessar jogos",
                    cor_botao = "LightSeaGreen",
                    cor_texto = "White",
                    local = "Meus Jogos",
                    icone = "search_blue",
                };
                Lista_Items item3 = new Lista_Items
                {
                    item = "Programação Clube Anexo",
                    item_index = 3,
                    texto_botao = "acessar pdf",
                    cor_botao = "#EC407A",
                    cor_texto = "White",
                    local = nome_anexo,
                    icone = "pdf_tab",
                };

                List<Lista_Items> Lista_Clube = new List<Lista_Items>();
                Lista_Clube.Add(item1);
                //quando houver programação em anexo
                if (anexo)
                {
                    listagem_pdf.HeightRequest = 220;
                    Lista_Clube.Add(item3);
                }
                Lista_Clube.Add(item2);

                if (!anexo) listagem_pdf.HeightRequest = 160;

                listagem_pdf.ItemsSource = Lista_Clube.ToList();

            }
            catch (Exception)
            {

            }

            UserDialogs.Instance.HideLoading();

        }


        public async void CarregarChaves_Torneio_Chamada(int id_etapa)
        {

            if (publico)
            {
                Lista_Categorias = await API_Service.ObterChaves_Torneio_Chamada(id_etapa);

                if (Lista_Categorias != null)
                {
                    if (Lista_Categorias.Count != 0)
                    {


                        if (Lista_Categorias[0].publico)
                        {
                            DadosAgrupados = Lista_Categorias.OrderBy(p => p.grupo_categoria)
                                                    .GroupBy(p => p.grupo_categoria.ToString())
                                                    .Select(p => new ObservableGroupCollection<string, Categoria>(p)).ToList();
                            lista_categorias.ItemsSource = DadosAgrupados;
                        }
                    }
                    else
                    {
                        stk_semregistro.IsVisible = true;
                        lista_categorias.IsVisible = false;
                        listagem_pdf.IsVisible = false;
                    }
                }
                else
                {
                    stk_semregistro.IsVisible = true;
                    lista_categorias.IsVisible = false;
                    listagem_pdf.IsVisible = false;
                }
            }
            else
            {
                stk_semregistro.IsVisible = true;
                lista_categorias.IsVisible = false;
                listagem_pdf.IsVisible = false;
            }



        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }

        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {

            var filePath = await DownloadPdfFileAsyncProgramacao(id_etapa, false);

            var uri = string.Format(filePath);
            await OpenBrowser(uri);

            //if (filePath != null)
            //{
            //    await Launcher.OpenAsync(new OpenFileRequest
            //    {
            //        File = new ReadOnlyFile(filePath)
            //    });
            //}

        }

        private async Task OpenBrowser(string uri)
        {
            try
            {
                await Browser.OpenAsync(uri, new BrowserLaunchOptions
                {
                    LaunchMode = BrowserLaunchMode.SystemPreferred,
                    TitleMode = BrowserTitleMode.Show,
                    PreferredToolbarColor = Color.AliceBlue,
                    PreferredControlColor = Color.Violet
                });
            }
            catch (Exception ex)
            {
                // An unexpected error occured. No browser may be installed on the device.
            }
        }

        private async void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {
            info_jogador = await API_Service.ObterDadosJogador(id_jogador);

            await Navigation.PushAsync(new Programacao_Tela(id_etapa, nome_etapa, info_jogador.Nome_Jogador));
        }


        private async Task<string> DownloadPdfFileAsyncProgramacao(int id_etapa, bool prg_anexo)
        {


            UserDialogs.Instance.ShowLoading(title: "Buscando Jogos");

            string url = "";

            var filePath = Path.Combine(FileSystem.AppDataDirectory, "Programação_" + nome_etapa + ".pdf");


            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                //return filePath;
            }

            ///Progrmacao/ProgramacaoPDF_Master?id=277&bool_anexo=false

            switch (id_tipo)
            {
                case 1:
                    if (prg_anexo) url = string.Format("https://tornfy.com/" + "/Progrmacao/ProgramacaoPDF_Master?id={0}&bool_anexo=false", id_etapa);
                    else url = string.Format("https://tornfy.com/" + "/Progrmacao/ProgramacaoPDF_Master?id={0}&bool_anexo=true", id_etapa);
                    break;
                case 2:
                    if (prg_anexo) url = string.Format("https://tornfy.com/" + "/Progrmacao/ProgramacaoPDF_Master?id={0}&bool_anexo=false", id_etapa);
                    else url = string.Format("https://tornfy.com/" + "/Progrmacao/ProgramacaoPDF_Master?id={0}&bool_anexo=true", id_etapa);
                    break;
                default:
                    break;
            }

            var uri = string.Format(url);
            await OpenBrowser(uri);

            //var httpClient = new HttpClient();
            //var pdfBytes = await httpClient.GetByteArrayAsync(url);

            //try
            //{
            //    File.WriteAllBytes(filePath, pdfBytes);
            //    UserDialogs.Instance.HideLoading();
            //    return filePath;
            //}
            //catch (Exception ex)
            //{
            //    await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
            //}

            UserDialogs.Instance.HideLoading();
            return null;
        }

        private async Task<string> DownloadPdfFileAsync(int id_cat, int id_etapa, string nome_chave, int id_tipoChave, int contagem, string nome_etapa)
        {

            UserDialogs.Instance.ShowLoading(title: "Buscando Jogos");

            string view_path = "";
            int id_tipochave = 0;

            if (id_tipoChave == 1)
            {
                if (contagem == 3) { id_tipochave = 3; view_path = "GrupoPDF_3"; }
                if (contagem == 4) { id_tipochave = 4; view_path = "GrupoPDF_4"; }
                if (contagem == 5) { id_tipochave = 5; view_path = "GrupoPDF_5"; }
                if (contagem == 6) { id_tipochave = 6; view_path = "GrupoPDF_6"; }
                if (contagem == 7) { id_tipochave = 7; view_path = "GrupoPDF_7"; }
                if (contagem == 8) { id_tipochave = 8; view_path = "GrupoPDF_8"; }
                if (contagem == 9) { id_tipochave = 9; view_path = "GrupoPDF_9"; }
                if (contagem == 10) { id_tipochave = 10; view_path = "GrupoPDF_10"; }
                if (contagem == 11) { id_tipochave = 11; view_path = "GrupoPDF_11"; }
                if (contagem == 12) { id_tipochave = 12; view_path = "GrupoPDF_12"; }
                if (contagem == 13) { id_tipochave = 13; view_path = "GrupoPDF_13_plus"; }
                if (contagem == 14) { id_tipochave = 14; view_path = "GrupoPDF_14"; }
                if (contagem == 15) { id_tipochave = 15; view_path = "GrupoPDF_15_plus"; }
                if (contagem == 16) { id_tipochave = 16; view_path = "GrupoPDF_16_plus"; }
                if (contagem == 17) { id_tipochave = 17; view_path = "GrupoPDF_17"; }
                if (contagem == 18) { id_tipochave = 18; view_path = "GrupoPDF_18_plus"; }
                if (contagem == 19) { id_tipochave = 19; view_path = "GrupoPDF_18_plus"; }
                if (contagem == 20) { id_tipochave = 20; view_path = "GrupoPDF_18_plus"; }
                if (contagem == 21) { id_tipochave = 21; view_path = "GrupoPDF_21_plus"; }
                if (contagem == 22) { id_tipochave = 22; view_path = "GrupoPDF_21_plus"; }
                if (contagem == 23) { id_tipochave = 23; view_path = "GrupoPDF_23_plus"; }
                if (contagem == 24) { id_tipochave = 24; view_path = "GrupoPDF_24_plus"; }
                if (contagem == 25) { id_tipochave = 25; view_path = "GrupoPDF_24_plus"; }
                if (contagem == 26) { id_tipochave = 26; view_path = "GrupoPDF_24_plus"; }
                if (contagem == 27) { id_tipochave = 27; view_path = "GrupoPDF_27_plus"; }
                if (contagem == 28) { id_tipochave = 28; view_path = "GrupoPDF_27_plus"; }
                if (contagem == 29) { id_tipochave = 29; view_path = "GrupoPDF_27_plus"; }
                if (contagem == 30) { id_tipochave = 30; view_path = "GrupoPDF_30_plus"; }
                if (contagem == 31) { id_tipochave = 31; view_path = "GrupoPDF_30_plus"; }
                if (contagem == 32) { id_tipochave = 32; view_path = "GrupoPDF_30_plus"; }

            }

            //direcionar para tipo de chave de eliminatória
            if (id_tipoChave == 2)
            {
                if (contagem > 32) { id_tipochave = 64; view_path = "ChavePDF_64"; }
                if (contagem < 33 && contagem > 16) { id_tipochave = 32; view_path = "ChavePDF_32_teste"; }
                if (contagem < 17 && contagem > 8) { id_tipochave = 16; view_path = "ChavePDF_16"; }
                if (contagem > 4 && contagem < 9) { id_tipochave = 8; view_path = "ChavePDF_8"; }
                if (contagem == 4) { id_tipochave = 4; view_path = "ChavePDF_4"; }
                if (contagem == 3) { id_tipochave = 3; view_path = "ChavePDF_3"; }
                if (contagem == 2) { id_tipochave = 2; view_path = "ChavePDF_2"; }
            }

            var filePath = Path.Combine(FileSystem.AppDataDirectory, nome_etapa + "_" + nome_chave + ".pdf");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                //return filePath;
            }


            var httpClient = new HttpClient();
            string url = string.Format("https://tornfy.com/Chave_PDF/{3}?id_classe={0}&id_etapa={1}&nome_chave={2}&id_tipochave={4}", id_cat, id_etapa, nome_chave, view_path, id_tipochave);

            var uri = string.Format(url);
            await OpenBrowser(uri);

            //var pdfBytes = await httpClient.GetByteArrayAsync(url);

            //try
            //{
            //    File.WriteAllBytes(filePath, pdfBytes);
            //    UserDialogs.Instance.HideLoading();
            //    return filePath;
            //}
            //catch (Exception ex)
            //{
            //    await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
            //}

            UserDialogs.Instance.HideLoading();
            return null;
        }

        private void lista_categorias_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private async void lista_categorias_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Categoria categoria = ((ListView)sender).SelectedItem as Categoria;
            if (categoria == null)
            {
                return;
            }
           
            var filePath = await DownloadPdfFileAsync(categoria.id_cat, categoria.id_etapa, categoria.Categoria_Nome, categoria.id_TipoSorteio, int.Parse(categoria.contagem), categoria.nome_etapa);

            if (filePath != null)
            {
                //await Launcher.OpenAsync(new OpenFileRequest
                //{
                //    File = new ReadOnlyFile(filePath)
                //});
                

            }
            


        }

        private void listagem_pdf_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private async void listagem_pdf_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Lista_Items item = ((ListView)sender).SelectedItem as Lista_Items;
            if (item == null)
            {
                return;
            }
            else
            {
                var filePath = "";

                switch (item.item_index)
                {
                    case 1:
                        filePath = await DownloadPdfFileAsyncProgramacao(id_etapa, true);
                        if (filePath != null)
                        {
                            //await Launcher.OpenAsync(new OpenFileRequest
                            //{
                            //    File = new ReadOnlyFile(filePath)
                            //});
                        }
                        break;
                    case 2:
                        info_jogador = await API_Service.ObterDadosJogador(id_jogador);
                        await Navigation.PushAsync(new Programacao_Tela(id_etapa, nome_etapa, info_jogador.Nome_Jogador));
                        break;
                    case 3:
                        filePath = await DownloadPdfFileAsyncProgramacao(id_etapa, false);
                        if (filePath != null)
                        {
                            //await Launcher.OpenAsync(new OpenFileRequest
                            //{
                            //    File = new ReadOnlyFile(filePath)
                            //});
                        }
                        break;
                    default:
                        break;
                }
            }

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            Lista_Items item = ((Button)sender).BindingContext as Lista_Items;
            if (item == null)
            {
                return;
            }
            else
            {
                var filePath = "";

                switch (item.item_index)
                {
                    case 1:
                        filePath = await DownloadPdfFileAsyncProgramacao(id_etapa, false);
                        if (filePath != null)
                        {
                            await Launcher.OpenAsync(new OpenFileRequest
                            {
                                File = new ReadOnlyFile(filePath)
                            });
                        }
                        break;
                    case 2:
                        info_jogador = await API_Service.ObterDadosJogador(id_jogador);
                        await Navigation.PushAsync(new Programacao_Tela(id_etapa, nome_etapa, info_jogador.Nome_Jogador));
                        break;
                    case 3:
                        filePath = await DownloadPdfFileAsyncProgramacao(id_etapa, true);
                        if (filePath != null)
                        {
                            await Launcher.OpenAsync(new OpenFileRequest
                            {
                                File = new ReadOnlyFile(filePath)
                            });
                        }
                        break;
                    default:
                        break;
                }
            }



        }
    }

    public class Lista_Items
    {

        public string item { get; set; }
        public string texto_botao { get; set; }
        public int item_index { get; set; }
        public string cor_texto { get; set; }

        public string cor_botao { get; set; }
        public string local { get; set; }
        public string titulo { get; set; }
        public string texto { get; set; }
        public string icone { get; set; }

    }
}