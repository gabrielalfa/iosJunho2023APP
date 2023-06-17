using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.PlatformConfiguration;

namespace TornfyApp.Model
{
    public class Quadra
    {
        public decimal valor_original { get; set; }
        public bool pgo_integral { get; set; }
        public int id_locacao { get; set; }
        public string lista_rateio { get; set; }

        public bool rateio { get; set; }
        public int rateio_limite { get; set; }
        public string gd_rateio { get; set; }
        public string valor_rateio { get; set; }
        public string parceiros { get; set; }


        public string path { get; set; }
        public string email { get; set; }

        public string bounce_1 { get; set; }
        public string bounce_2 { get; set; }

        public int id_proprietario { get; set; }
        public int id_esquema_horario { get; set; }
        public bool isentar { get; set; }
        public bool bool_isentar { get; set; }
        public string isento_string { get; set; }
        public bool existe { get; set; }
        public string nome_complexo { get; set; }
        public string telefone { get; set; }
        public string endereco { get; set; }
        public string descricao { get; set; }

        public string qrcode { get; set; }
        public string qr_linha { get; set; }
        public string codigo { get; set; }
        public bool status_pagarme { get; set; }
        public string lista_pagamentos { get; set; }
        public string total_pgo { get; set; }
        public string color_valor { get; set; }
        public string status_pgo_final { get; set; }
        public string string_pgo { get; set; }
        public string texto_locar { get; set; }
        public string color_botao { get; set; }
        public string string_quadra { get; set; }
        public string mensagem { get; set; }
        public int codigo_retorno { get; set; }
        public string titulo { get; set; }

        public string nome_locacoes { get; set; }
        public bool status_constante { get; set; }
        public bool pgo_luz { get; set; }
        public bool pgo_quadra { get; set; }

        public bool cobranca_quadra { get; set; }
        public string descricao_cobranca { get; set; }
        public bool status_pagamento { get; set; }
        public int tipo_cobranca { get; set; }
        public bool bool_cobranca { get; set; }
        public string valor_quadra { get; set; }
        public string valor_iluminacao { get; set; }
        public int id_master { get; set; }
        public string valor_cobranca { get; set; }
        public DateTime DayOfYear { get; set; }
        public string gd_constante { get; set; }
        public string periodo { get; set; }
        public string nome_marcacao { get; set; }
        public string dia_string { get; set; }
        public int id { get; set; }
        public string data { get; set; }
        public string horario { get; set; }
        public bool status { get; set; }
        public int id_jogador { get; set; }
        public int id_complexo { get; set; }
        public int id_quadra { get; set; }
        public int id_clube { get; set; }

        public bool vitoria { get; set; }
        public string resultado { get; set; }
        public string nome_clube { get; set; }

        public int id_tipo_quadra { get; set; }
        public int id_esquema { get; set; }

        public bool visivel { get; set; }
        public string Nome_Quadra { get; set; }
        public string tipo_quadra { get; set; }
        public string Nome_Esquema { get; set; }
        public string numero_q { get; set; }
        public int codigo_jogador { get; set; }
        public string nome_jogador { get; set; }

    }
}
