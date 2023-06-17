using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TornfyApp.Model
{
    public class Inscricao
    {
        public string responsavel { get; set; }
        public int id_tipo { get; set; }
        public string status_pgo_final { get; set; }
        public string color { get; set; }
        public string pagamento_string { get; set; }
        public string path_pagamento { get; set; }
        public int contagem_n_pagos { get; set; }

        public decimal somatotal { get; set; }
        public decimal pagas { get; set; }
        public decimal total_pgo { get; set; }
        public decimal credito { get; set; }
        public bool valor_zero { get; set; }
        public bool pagar_com_Credito_Saldo { get; set; }



        public bool tipo_simples { get; set; }
        public bool is_dupla { get; set; }
        public string saldo { get; set; }
        public string transaction_dupla_id { get; set; }
        public string transaction_id { get; set; }
        public string charge_id { get; set; }
        public string charge_id_dupla { get; set; }
        public string valor_inscricao_1 { get; set; }
        public string valor_inscricao_2 { get; set; }
        public string valor_inscricao_3 { get; set; }

        public bool infantil { get; set; }
        public int id_cat_parcial { get; set; }
        public int qtd { get; set; }
        public bool adm { get; set; }
        public int id_parceiro { get; set; }
        public string letra { get; set; }
        public int id { get; set; }

        public string camiseta_tamanho { get; set; }

        public int status_pix { get; set; }
        public string valor_transaction { get; set; }
        public string Nome_Etapa { get; set; }
        public string Nome_Clube { get; set; }
        public string Categoria { get; set; }
        public int id_jogador { get; set; }
        public string img_path { get; set; }
        public string telefone { get; set; }


        public int id_torneio { get; set; }
        public int ano_lista { get; set; }
        public string arbitragem { get; set; }
        public string inicio { get; set; }
        public string string_clube { get; set; }
        public string nome_jogador { get; set; }
        public string nome_dupla { get; set; }

        public string Nome_Jogador1 { get; set; }

        public string Nome_Jogador2 { get; set; }

        public int id_ranking { get; set; }
        public int id_temporada { get; set; }

        public bool status_pagamento_dupla { get; set; }
        public bool status_pagamento { get; set; }
        public bool status_negra { get; set; }
        public bool status_brinde { get; set; }
        public bool status_brinde_dupla { get; set; }
        public bool status_isento { get; set; }


        public int contagem_total { get; set; }
        public int pagas_adulto { get; set; }
        public int pagas_infantil { get; set; }
        public int brides { get; set; }
        public int lista_negra { get; set; }
        public int isentos { get; set; }
        public int arrecadacao_adulto { get; set; }
        public int arrecadacao_infantil { get; set; }

        public int id_categoria { get; set; }

        public string email { get; set; }
        public int id_dupla { get; set; }

        public int c_2 { get; set; }
        public int c_1 { get; set; }
        public int id_cat_geral { get; set; }

        public string valor { get; set; }
        public string valor_dupla { get; set; }

        public string url_checkout { get; set; }

        public int id_grupo { get; set; }

        public int Ranking { get; set; }
        public int id_etapa { get; set; }

        public bool status_carrinho { get; set; }
        public string nome_dupla_gb { get; set; }
        public string nome_clubes_dupla { get; set; }
        public Inscricao BindingContext { get; internal set; }
    }
}