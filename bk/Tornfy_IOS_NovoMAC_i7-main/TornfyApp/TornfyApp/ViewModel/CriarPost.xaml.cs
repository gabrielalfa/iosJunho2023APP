using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;



namespace TornfyApp.ViewModel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CriarPost : ContentPage
    {
        public ObservableCollection<string> GalleryImages { get; set; }
        private ObservableCollection<ImageSource> _galleryImages = new ObservableCollection<ImageSource>();


        public CriarPost(int id, int tipo_page)
        {
            InitializeComponent();
            if (tipo_page == 1)
            {
                int id_jogador = id;
                // pagina de criação
                webView.Source = "https://tornfy.com/Feed/CriarPost_WebView?id_jogador=" + id_jogador;
            }
            else if (tipo_page == 2)
            {
                int id_post = id;
                // pagina de edicao
                webView.Source = "https://tornfy.com/Feed/EditarPost_WebView?id_post=" + id_post;
            }

        }


    }
}