using System;
using System.Collections.Generic;
using System.Text;

namespace TornfyApp.Model
{
    public partial class Jogador
    {
        public bool recente { get; set; }
        public string mensagem { get; set; }
        public bool status_locacao { get; set; }
        public bool bool_acesso { get; set; }
        public string string_clube_volei { get; set; }
        public string string_clube_fut { get; set; }
        public int clube_volei { get; set; }
        public int clube_fut { get; set; }

        public int tipo_clube { get; set; }
        public string path_clube_j1 { get; set; }
        public string path_clube_j2 { get; set; }

        public int tipo_etapa { get; set; }
        public int id_master { get; set; }
        public string string_pagamento { get; set; }
        public string transaction_dupla_id { get; set; }
        public string transaction_id { get; set; }
        public bool status_pagamento { get; set; }
        public bool status_pagamento_dupla { get; set; }

        public string color_font { get; set; }
        public string color_inscricao { get; set; }
        public string situacao_inscricao { get; set; }
        public int contagem { get; set; }
        public int limite_cat { get; set; }
        public string gd_jogador { get; set; }
        public string gd_clube { get; set; }
        public bool lista_espera { get; set; }
        public int id_dupla { get; set; }
        public int pontos_dupla { get; set; }
        public int pontos { get; set; }
        public string Nome_Jogador_simples { get; set; }
        public bool tipo_simples { get; set; }
        public bool confimacao_email { get; set; }
        public string Nome_clube_beach { get; set; }
        public string clube_beach_string { get; set; }
        public string path_clube_beach { get; set; }

        public int id_clube_beach { get; set; }
        public string vitorias { get; set; }
        public string derrotas { get; set; }
        public string gd { get; set; }
        public int avatar { get; set; }
        public int posicao { get; set; }
        public bool suplente_jogador { get; set; }
        public int associados_id { get; set; }
        public string Nomedupla_jogador { get; set; }
        public int id_Jogador { get; set; }
        public string Nome_Jogador { get; set; }
        public string Nome_Clube { get; set; }
        public int id_Clube { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Path { get; set; }
        public string categoria { get; set; }
        public string path_clube { get; set; }
        public string nome_cat { get; set; }
        public int id_cat { get; set; }
        public string Jogador_Email { get; set; }
        public string Data_Nascimento { get; set; }
        public string password { get; set; }
        public string telefone { get; set; }
        public bool status_cadastro { get; set; }
        public string sexo { get; set; }
        public string String_Clube { get; set; }
        public string Sexo { get; set; }
        public bool Atualizacao_2016 { get; set; }
        public bool Atualizacao_2016_beach { get; set; }
        public string clube_beach { get; set; }
        public int id_sexo { get; set; }
        public string Sexo_s { get; set; }

        public string pub_perfor { get; set; }
        public string aviso_email { get; set; }
        public string convite_email { get; set; }
        public int id_inscricao { get; set; }
        public int cont_total { get; set; }


        public int id_jogador { get; set; }
        public int id { get; set; }
        public bool status { get; set; }
        public string id_etapa { get; set; }
        public string CPF { get; set; }

        public int id_temporada { get; set; }

        public string nome_dupla_gb { get; set; }
        public string nome_clubes_dupla { get; set; }
        public int id_ranking { get; set; }
        public string cidade { get; set; }
        public string Nome_Beach_simples { get; set; }
        public int id_modelo { get; set; }
    }
}
