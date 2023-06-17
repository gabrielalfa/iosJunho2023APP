using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Inteleco.Models
{
    public class Categoria
    {
        public string nome_etapa { get; set; }
        public string img_ball { get; set; }
        public string grupo_categoria { get; set; }
        public string contagem { get; set; }
        public int id_tipo_etapa { get; set; }
        public bool all { get; set; }
        public string tipo_simples_string { get; set; }
        public string infantil_string { get; set; }
        public int tipo_simples_int { get; set; }
        public bool tipo_simples { get; set; }
        public string valor_inscricao_1 { get; set; }
        public string valor_inscricao_2 { get; set; }
        public string valor_inscricao_3 { get; set; }
        public string valor_inscricao_inf { get; set; }

        public int id_infatil { get; set; }
        public bool infantil { get; set; }
        public bool pontuar { get; set; }
        public string letra { get; set; }
        public int id_master { get; set; }
        public int id { get; set; }
        public string Categoria_Nome { get; set; }
        public string num { get; set; }
        public bool build { get; set; }
        public string Lista_categorias { get; set; }
        public string check_bool { get; set; }
        public int id_etapa { get; set; }
        public int id_cat { get; set; }
        public int id_cam { get; set; }
        public int id_dupla { get; set; }

        public int id_TipoSorteio { get; set; }
        public bool publico { get; set; }
        public bool Cat_valida { get; set; }

        public string id_string { get; set; }
        public int id_modelo { get; set; }
        public int Ranking { get; set; }

        public int id_jogador { get; set; }
        public int id_temporada { get; set; }

        public int id_grupo { get; set; }
        public bool status_carrinho { get; set; }
        public bool status { get; set; }
        public int id_clube { get; set; }
        public int id_clube2 { get; set; }
    }
}