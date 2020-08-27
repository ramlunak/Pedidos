using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Models
{
    public class P_Ventas
    {
        public int id { get; set; }
        public int idProducto { get; set; }        
        public DateTime fecha { get; set; }
        public float Precio { get; set; }
        public int idCuenta { get; set; }
        public bool activo { get; set; }
    }
}
