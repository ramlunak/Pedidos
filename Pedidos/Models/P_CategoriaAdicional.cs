using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Models
{
    public class P_CategoriaAdicional
    {
        public int id { get; set; }

        public int idCategoria { get; set; }
        
        public string idsAdicionales { get; set; }       

        public int idCuenta { get; set; }
    }
}
