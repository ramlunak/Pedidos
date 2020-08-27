using Pedidos.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Models
{
    public class P_Mesa
    {
        public int id { get; set; }     
        public string nombre { get; set; }
        public StatusMesa status{ get; set; }
        public int idCuenta { get; set; }
        public bool activo { get; set; }
    }
}
