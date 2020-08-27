using Pedidos.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Models
{
    public class P_Estoque
    {
        public int id { get; set; }
        public int idProduto { get; set; }
        public int cantidad { get; set; }
        public float costo { get; set; }
        public UnidadMedida unidadeMedida { get; set; }
        public DateTime fechaEntrada { get; set; }
        public int idCuenta { get; set; }
        public bool activo { get; set; }
    }
}
