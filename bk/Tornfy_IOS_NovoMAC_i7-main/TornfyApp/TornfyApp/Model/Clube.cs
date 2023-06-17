using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Inteleco.Models
{
    public class Clube
    {

        public string item { get; set; }
        public string texto_botao { get; set; }
        public int item_index { get; set; }



        public string proprietario { get; set; }
        public string gd { get; set; }
        public string UF { get; set; }
        public string Estado { get; set; }
        public int id { get; set; }
        public string Nome_CLube { get; set; }
        public string path { get; set; }
        public string Telefone { get; set; }
        public int pontos { get; set; }
        public string cidade { get; set; }
        public string email { get; set; }
        public string String_Clube { get; set; }
        public string Responsavel { get; set; }

        public string img_apresentacao { get; set; }

        //========================================

        public bool id_locacao { get; set; }
        public bool id_cobranca { get; set; }
        public bool id_iluminacao { get; set; }

        public bool id_restaturante { get; set; }
        public bool id_recibo { get; set; }

        public bool id_coberta { get; set; }
        public bool id_associado { get; set; }
        public int id_cidade { get; set; }
        public string Password { get; set; }

        //========================================
    }
}