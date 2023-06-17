using ICT.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace TornfyApp.Model
{
    public class LoginViewModel
    {

        public LoginViewModel()
        {
            Onbording = GetOnbordings();

        }

        public List<Onbording> Onbording { get; set; }

        public List<Onbording> GetOnbordings()
        {
            return new List<Onbording>
            {
                new Onbording{ img="court.jpg",  Caption = "Fácil e intuitivo, você encontra as melhores competições na palma da mão :)", Heading="Acessar meus torneios"},
                new Onbording{ img="raquetes.jpg", Caption = "Competições de Tênis, Beach Tênis e Esportes de Areia estão aqui :)", Heading="Tênis e Esportes de Areia"},
                new Onbording{ img="balls_foto.jpg", Caption = "Faça seu cadastro ou seu login para pode acessar todo o conteúdo do ICT :)", Heading="Faça seu Cadastro"},
            };
        }
       
    }


    public class Onbording
    {
        public string img { get; set; }

        public string Heading { get; set; }
        public string Caption { get; set; }
    }

}
