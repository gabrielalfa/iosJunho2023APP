using System;
namespace TornfyApp.Model
{
    public class Credito
    {
        public decimal total_creditos { get; set; }
        public string nome_complexo { get; set; }
        public string credito { get; set; }
        public int id { get; set; }
        public int id_jogador { get; set; }
        public int id_master { get; set; }
        public string data { get; set; }
        public int acao_tipo { get; set; }
        public string task { get; set; }
        public string hora { get; set; }
        public string valor { get; set; }
    }
}

