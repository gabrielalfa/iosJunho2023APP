using System;
using System.Collections.Generic;
using System.Text;

namespace TornfyApp.Model
{
    public class FAQ
    {

        public int id { get; set; }
        public int id_tipo { get; set; }
        public string questao { get; set; }
        public string resposta { get; set; }
        public string url { get; set; }
        public bool url_bool { get; set; }
        public string mascara_url { get; set; }


        public string nome { get; set; }
        public string email { get; set; }
        public string telefone { get; set; }
        public string mensagem { get; set; }


    }
}
