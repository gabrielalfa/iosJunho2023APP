using System;
using System.Collections.Generic;
using System.Text;

namespace TornfyApp.Model
{
    public class Temporada
    {
        public int qtd_etp_abertas { get; set; }
        public int qtd_etp { get; set; }
        public int id_del { get; set; }
        public bool etp_fav { get; set; }
        public bool circ_fav { get; set; }
        public string uri { get; set; }

        public int id_grupo { get; set; }
        public int tipo_etapa { get; set; }
        public int tipo_ranking { get; set; }

        public string tipo_etapa_nome { get; set; }
        public string circuito { get; set; }
        public string label_inscricao { get; set; }
        public string label_circuito { get; set; }
        public string info { get; set; }
        public string data { get; set; }
        public string nome_mes_inicio { get; set; }
        public string color { get; set; }
        public string Nome_Etapa { get; set; }
        public string limite { get; set; }
        public string situacao { get; set; }
        public string nome_grupo { get; set; }
        public string path_grupo { get; set; }


        public bool favorito { get; set; }
        public int id_tipo_etapa { get; set; }
        public string proprietario { get; set; }
        public int id_master { get; set; }
        public int id_temporarda { get; set; }
        public string banner_desk { get; set; }
        public string banner_mobile { get; set; }
        public bool status_desk { get; set; }
        public bool status_mobile { get; set; }

        public int id_ranking { get; set; }
        public string path { get; set; }
        public string criador { get; set; }
        public bool status { get; set; }
        public string grupo_nome { get; set; }

        public int id { get; set; }

        public string Nome_temporada { get; set; }

        public string Ano { get; set; }

        public int Ranking { get; set; }

        public DateTime Inicio { get; set; }

        public DateTime Fim { get; set; }
        //[Required(ErrorMessage = "Escolha tipo de pontuação")]
        public int Pontuacao { get; set; }
        //public string Fim_temporada { get; set; }
        public string Nome_Ranking { get; set; }
        public bool status_ranking { get; set; }




    }
}
