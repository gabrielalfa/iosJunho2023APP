using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TornfyApp.Model
{
    public class Formulario_Inscricao
    {

        public string tamanho { get; set; }
        public string tamanho_dupla { get; set; }
        public int id_etapa { get; set; }
        public int id_jogador { get; set; }
        public int id_cat { get; set; }
        public int id_dupla { get; set; }
        public string email { get; set; }
        public string cpf { get; set; }

    }
}