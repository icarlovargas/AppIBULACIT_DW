﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppIBULACIT.Models
{
    public class Moneda
    {
        
        public Moneda()
        {
            this.Cuenta = new HashSet<Cuenta>();
        }

        public int Codigo { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }

        
        public virtual ICollection<Cuenta> Cuenta { get; set; }
    }
}