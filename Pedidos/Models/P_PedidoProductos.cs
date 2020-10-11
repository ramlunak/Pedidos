using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Models
{
    public class P_PedidoProductos
    {
        public int id { get; set; }
        public int idPedido { get; set; }
        public int idProducto { get; set; }
        public decimal valorProducto { get; set; }
    }
}
