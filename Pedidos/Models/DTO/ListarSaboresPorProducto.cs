using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Models.DTO
{
    public class ListarSaboresPorProducto
    {
        public int idProducto { get; set; }

        public int idSabor { get; set; }

        public string sabor { get; set; }

        public decimal? valor { get; set; }

        public bool selected { get; set; }
    }
}
