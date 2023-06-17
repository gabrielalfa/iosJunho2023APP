using System;
using System.Collections.Generic;
using System.Text;

namespace TornfyApp.Model
{
    public class Ranking
    {
        public string proprietario { get; set; }
        public string grupo_categoria { get; set; }
        public string color_font { get; set; }
        public string color_back { get; set; }
        public int contagem { get; set; }
        public string tipo_ranking_nome { get; set; }
        public int tipo_ranking { get; set; }
        public string banner_desk { get; set; }
        public string banner_mobile { get; set; }
        public bool status_desk { get; set; }
        public bool status_mobile { get; set; }
        public string tipo_cat { get; set; }
        public bool muc { get; set; }
        public bool mpc { get; set; }
        public bool suplente { get; set; }

        public int posicao { get; set; }
        public int posicao_temp { get; set; }

        public int avatar { get; set; }
        public int id_dupla { get; set; }
        public int vs1_p2 { get; set; }
        public int vs2_p1 { get; set; }
        public int grupo_chaves { get; set; }
        public int id_j1 { get; set; }
        public int c1 { get; set; }
        public int id_j2 { get; set; }
        public int c2 { get; set; }

        public int id { get; set; }

        public string Nome_Ranking { get; set; }


        public string Nome_fase { get; set; }
        public int Rankings { get; set; }


        public int pt_Campeao { get; set; }

        public int pt_Semi { get; set; }

        public int pt_Quartas { get; set; }

        public int pt_Oitavas { get; set; }

        public int pt_Dezeseis { get; set; }

        public int pt_32 { get; set; }

        public int pt_64 { get; set; }

        //===============


        public int pt_grupo { get; set; }
        public int pt_desafio { get; set; }
        public bool pt_wo_gr { get; set; }
        public bool pt_wo_ds { get; set; }


        //===============
        public string Nome_Clube { get; set; }
        public int sumd { get; set; }
        public string path_clube { get; set; }
        public string Nome_Jogador { get; set; }
        public string Nome_Categoria { get; set; }
        public int Clube_id { get; set; }
        public int Soma_Jogador { get; set; }
        public string img_path { get; set; }
        public int Pontuacao_temp { get; set; }
        public int Pontuacao_dif { get; set; }
        public string Pontuacao { get; set; }
        public int id_jogador { get; set; }
        public int id_linha_rk { get; set; }
        public int id_modelo { get; set; }
        public int id_grupo { get; set; }
        public bool ativo { get; set; }
        public int id_categoria { get; set; }

        public string String_Clube { get; set; }



        //==============saldo

        public string gd { get; set; }
        public int saldo_confrontos { get; set; }
        public int saldo_games { get; set; }
        public int id_clube { get; set; }
        public int row { get; set; }
        public int pt { get; set; }

        public int ano { get; set; }
        public int torneio { get; set; }
        public string Nome_Etapa { get; set; }

        public int pt_temp { get; set; }

        //==============saldo



        //==============desafios

        public int pt_des_vt { get; set; }
        public int pt_des_dr { get; set; }
        public int pt_des_set_ven { get; set; }
        public int pt_des_game_ven { get; set; }
        public int pt_des_tie_ven { get; set; }
        public int pt_des_WO_ven { get; set; }
        public bool pt_des_pont_WO { get; set; }

        //==============desafios

        public int pt_prima_rodada { get; set; }
        public int saldo_prima_rodada { get; set; }

        //================Desafios RJ


        public int id_grupos_class { get; set; }
        public int id_geral_class { get; set; }
        public string Categoria { get; set; }
        public int qtd_jogos { get; set; }
        public int qtd_games_win { get; set; }
        public int qtd_games_lose { get; set; }
        public int aproveitamento { get; set; }
        public string Nome_Grupo { get; set; }

        //==================Desafios RJ Fim

        public int id_cat_geral { get; set; }
        public int id_inscricao { get; set; }
        public int id_etapa { get; set; }

    }
}
