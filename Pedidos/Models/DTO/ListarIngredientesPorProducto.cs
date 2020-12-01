using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Models.DTO
{
    public class ListarIngredientesPorProducto
    {       
        public int idProducto { get; set; }
        
        public int idIngrediente { get; set; }

        public string ingrediente { get; set; }

        public bool selected { get; set; }

    }
}
