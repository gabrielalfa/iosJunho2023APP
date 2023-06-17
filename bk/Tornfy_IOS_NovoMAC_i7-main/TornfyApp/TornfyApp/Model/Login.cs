using System;
using System.Collections.Generic;
using System.Text;

namespace TornfyApp.Model
{
    public class Login
    {

        public int id { get; set; }

        public string gd { get; set; }
        public int id_jogador { get; set; }
        public string nome_jogador { get; set; }

        public string email { get; set; }
        public string senha { get; set; }
        public int id_propietario { get; set; }

    }
}
