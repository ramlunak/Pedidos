using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Models.DTO
{
    public class ListarAdicionalesPorProducto
    {
        public int idProducto { get; set; }

        public int idAdicional { get; set; }

        public string adicional { get; set; }

        public bool selected { get; set; }
    }
}
