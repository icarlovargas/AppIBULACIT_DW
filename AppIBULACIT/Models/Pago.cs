using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppIBULACIT.Models
{
    public class Pago
    {
        public int Codigo { get; set; }
        public int CodigoServicio { get; set; }
        public int CodigoCuenta { get; set; }
        public int CodigoMoneda { get; set; }
        public System.DateTime FechaHora { get; set; }
        public decimal Monto { get; set; }
    }
}