using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Models.DTO
{
    public class ListarCategoriasPorAdicional
    {
        public int idAdicional { get; set; }

        public int idCategoria { get; set; }
        
        public string categoria { get; set; }        
        
        public bool selected { get; set; }

    }
}
