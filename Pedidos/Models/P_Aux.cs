using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Models
{
    public class P_Aux
    {
        //CARGAR CARDAPIO
        public int id { get; set; }
        public string Categoria { get; set; }
        public string JsonProducto { get; set; }
        public string JsonAdicionales { get; set; }
        public string JsonIngredientes { get; set; }
    }

    public class ResultJson
    {        
        public P_Adicionais[] A { get; set; }
    }
}
