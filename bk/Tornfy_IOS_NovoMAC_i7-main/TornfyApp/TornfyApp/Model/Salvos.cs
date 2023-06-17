using System;
using System.Collections.Generic;
using System.Text;

namespace TornfyApp.Model
{
    public class Salvos
    {
        public bool anexo { get; set; }
        public string nome_anexo { get; set; }
        public string nome_principal { get; set; }

        public bool publico_programacao { get; set; }
        public int id_tipo { get; set; }
        public string Nome_Ranking { get; set; }
        public string Nome_Etapa { get; set; }
        public string grupo_nome { get; set; }
        public string path_grupo { get; set; }

        public int id_ranking { get; set; }
        public string circuito { get; set; }
        public string info { get; set; }

        public string label_inscricao { get; set; }
        public string label_circuito { get; set; }
        public bool favorito { get; set; }

        public string color { get; set; }
        public string situacao { get; set; }
        public string opacity { get; set; }


        public string data { get; set; }
        public string nome_mes_inicio { get; set; }
        public int id_grupo { get; set; }
        public bool publico { get; set; }
        public int id { get; set; }
        public int id_jogador { get; set; }
        public int id_etapa { get; set; }
        public int id_circuito { get; set; }
        public bool etapa_bool { get; set; }
        public bool circuito_bool { get; set; }
        public string nome_etapa { get; set; }
        public string inicio { get; set; }
        public string fim { get; set; }
        public string img_path { get; set; }
        public string nome_grupo { get; set; }
        public string limite { get; set; }
        public bool andamento { get; set; }
        public string uri { get; set; }

    }
}
