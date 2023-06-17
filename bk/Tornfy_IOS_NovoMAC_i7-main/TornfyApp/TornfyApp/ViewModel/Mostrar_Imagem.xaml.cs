using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TornfyApp.ViewModel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Mostrar_Imagem : ContentPage
    {
        public Mostrar_Imagem(string img_path)
        {
            InitializeComponent();


            image_cartaz.Source = img_path;
        }
    }
}