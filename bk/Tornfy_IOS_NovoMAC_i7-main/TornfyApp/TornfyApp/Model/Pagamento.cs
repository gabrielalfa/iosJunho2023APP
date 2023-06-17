using System;
using System.Collections.Generic;
using System.Text;

namespace TornfyApp.Model
{
    public class Pagamento
    {
        public string QrCode { get; set; }
        public string QrCodeUrl { get; set; }
        public bool status_pagarme { get; set; }
        public string codigo { get; set; }
        public string charge_codigo { get; set; }
    }
}
