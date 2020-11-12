using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Models
{
    public class P_AdicionalCategorias
    {
        public int id { get; set; }

        public int idAdicional { get; set; }

        public string idsCategoria { get; set; }

        public int idCuenta { get; set; }
    }
}
