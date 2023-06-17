using System;
using System.Collections.Generic;
using System.Text;

namespace TornfyApp.Model
{
    public partial class Etapa
    {
        public string nome_mes_limite { get; set; }
        public int valor_credito { get; set; }
        public bool publico_anexo { get; set; }
        public string nome_anexo { get; set; }
        public string nome_principal { get; set; }

        public bool etapa_bool { get; set; }
        public int total_pgo { get; set; }
        public string lista_idInscricoes { get; set; }
        public string QrCode { get; set; }
        public string QrCodeUrl { get; set; }
        public bool status_pagarme { get; set; }

        public string label_inscricao { get; set; }
        public string label_circuito { get; set; }

        public bool favorito { get; set; }
        public string nome_mes_inicio { get; set; }
        public string nome_mes_inicio_grupo { get; set; }
        public string master { get; set; }
        public string path { get; set; }
        public string circuito { get; set; }
        public string obs { get; set; }
        public string info { get; set; }

        public string proprietario { get; set; }
        public string nome_botao { get; set; }
        public string Description { get; set; }
        public int? type; // Represents the object_type column from my database
        public List<int?> objects; // Represents the object_id column from my database
        public string tamanho_dupla { get; set; }
        public string tamanho { get; set; }
        public string opacity { get; set; }
        public string img_cartaz { get; set; }
        public string data { get; set; }
        public string color { get; set; }
        public string situacao { get; set; }
        public string limite { get; set; }

        public int id_clube_fltro { get; set; }
        public bool andamento { get; set; }
        public string color_inscricao { get; set; }
        public string situacao_inscricao { get; set; }
        public bool banner_patrocinio { get; set; }
        public string concat { get; set; }
        public string Data_json { get; set; }
        public bool maior { get; set; }
        public int id_tipo { get; set; }
        public int id_cam { get; set; }
        public int id_dupla { get; set; }
        public string Telefone_Contato_pix { get; set; }
        public string gd { get; set; }
        public string responsavel { get; set; }
        public string img_path_patrocinio { get; set; }
        public string observacao { get; set; }
        public string path_grupo { get; set; }
        public int id_ranking { get; set; }
        public int contagem { get; set; }
        public string valor_inscricao_1 { get; set; }
        public string valor_inscricao_2 { get; set; }
        public string valor_inscricao_3 { get; set; }
        public string valor_inscricao_inf { get; set; }
        public bool pontuar { get; set; }
        public string nome_grupo { get; set; }
        public string tipo_chave { get; set; }
        public string chave_pix { get; set; }
        public int id { get; set; }
        //[Required(ErrorMessage = "Nome Etapa")]
        public string Nome_Etapa { get; set; }
        //[Required(ErrorMessage = "Defina data Limite")]
        public string Data_Limite { get; set; }
        public int Ranking { get; set; }

        public int num_limite { get; set; }
        //[Required(ErrorMessage = "Início Etapa")]
        public string Inicio { get; set; }
        //[Required(ErrorMessage = "Fim Etapa")]
        public string Fim { get; set; }
        public int Pontuacao { get; set; }
        public int Pontuacao_Saldo { get; set; }
        //[Required(ErrorMessage = "Nome Arbitragem obrigatório")]
        public string Arbitragem { get; set; }
        //[Required(ErrorMessage = "Telefone de Contato obrigatório")]
        //[MinLength(10, ErrorMessage = "Expressão correta (00)0000-0000")]
        public string Telefone_Contato { get; set; }
        public string valor { get; set; }
        public string Premiacao { get; set; }

        public string img_path { get; set; }
        public int id_temporada { get; set; }
        public int id_master { get; set; }
        public string Nome_clube { get; set; }

        public int Rankings { get; set; }

        public string Listagem { get; set; }
        public int id_cat { get; set; }
        public bool check { get; set; }
        public int contagem_cat { get; set; }

        public bool publico { get; set; }
        public string Nome_Ranking { get; set; }
        public string Impedimentos { get; set; }
        public int total_inscritos { get; set; }
        public int num_limite_parcial { get; set; }
        public string string_clube { get; set; }
        public string path_clube { get; set; }
        public string id_clube { get; set; }
        public string Nome_Categoria { get; set; }
        public string bool_filho { get; set; }
        public string total_inscritos_display { get; set; }
        public string num_limite_display { get; set; }
        public string id_mapa { get; set; }
        public string id_mapa_anexo { get; set; }
        public int id_jogador { get; set; }
        public bool publico_programacao { get; set; }
        public bool publico_pix { get; set; }
        public bool anexo { get; set; }
        public bool etapa { get; set; }

        public int id_etapa { get; set; }
        public int id_grupo { get; set; }
        public int id_modelo { get; set; }

        public bool show_pub { get; set; }
        public int Ano { get; set; }
        public string url_checkout { get; set; }







    }
}
