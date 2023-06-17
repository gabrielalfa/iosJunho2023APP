using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TornfyApp.Model
{
    public class Stats
    {

        public string rating_elo { get; set; }
        public string item { get; set; }

        public string texto_botao { get; set; }

        public int item_index { get; set; }
        public string nome_categoria { get; set; }
        public string color_arrow { get; set; }
        public string mp { get; set; }
        public int per_vitorias1 { get; set; }
        public int per_set1 { get; set; }
        public int per_games1 { get; set; }
        public int pontuacao_atual1 { get; set; }

        public int mes { get; set; }
        public string fim { get; set; }
        public int id { get; set; }
        public int id_temporada { get; set; }
        public string ano { get; set; }
        public string Nome_Jogador { get; set; }

        public int SomaVitorias { get; set; }
        public int SomaDerrotas { get; set; }
        public int SomaGamesGanhos_e { get; set; }
        public int SomaGamesGanhos_d { get; set; }
        public int TotalPartidas { get; set; }
        public int SomaGamesGanhos { get; set; }
        public int SomaGamesPerdidos { get; set; }
        public int Total_Games { get; set; }

        public int SomaSetGanhos { get; set; }
        public int SomaSetPerdidos { get; set; }
        public int Total_Sets { get; set; }
        public string id_jogador1 { get; set; }
        public string id_jogador2 { get; set; }




        public string Nome_Etapa { get; set; }
        public int total_etapa { get; set; }
        public int total_cat { get; set; }
        public string melhor_posicao { get; set; }
        public int sets_etapa { get; set; }
        public int games_etapa { get; set; }
        public string vit_etapa_e { get; set; }
        public string vit_etapa_d { get; set; }
        public string der_etapa_e { get; set; }
        public string der_etapa_d { get; set; }

        public int performance { get; set; }

        public int vit_etapa { get; set; }
        public int der_etapa { get; set; }

        //strings

        public string string_SomaGamesGanhos_e { get; set; }
        public string string_SomaGamesGanhos_d { get; set; }
        public string string_SomaGamesPerdidos_e { get; set; }
        public string string_SomaGamesPerdidos_d { get; set; }
        public string string_SomaSetGanhos_e { get; set; }
        public string string_SomaSetGanhos_d { get; set; }
        public string string_SomaSetPerdidos_e { get; set; }
        public string string_SomaSetPerdidos_d { get; set; }

        //strings


        public int performance_sets { get; set; }
        public int performance_games { get; set; }
        public int performance_vitorias { get; set; }


        public int participacoes1 { get; set; }
        public int pontuacao_Atual1 { get; set; }

        public int participacoes2 { get; set; }
        public int pontuacao_Atual2 { get; set; }

        public int Percent_Medio1 { get; set; }
        public int Percent_Medio2 { get; set; }


    }
}
