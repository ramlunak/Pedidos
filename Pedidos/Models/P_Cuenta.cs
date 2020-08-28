using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Models
{
    public class P_Cuenta
    {
        public int id { get; set; }
        public string usuario { get; set; }
        public string password { get; set; }       
        public string rol { get; set; }       
        public int? idPlano { get; set; }
        public bool activo { get; set; } = true;
    }
}
