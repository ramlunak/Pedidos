using Pedidos.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Models
{
    public class P_Pedidos
    {
        public int id { get; set; }
        public int? idCliente { get; set; }
        public int? idMesa { get; set; }
        public int? idAplicativo { get; set; }
        public DateTime fecha { get; set; }
        public StatusPedido status { get; set; }
        public string Descripcion { get; set; }
        public int idCuenta { get; set; }
        public bool activo { get; set; }

        [NotMapped]
        public List<P_Ventas> Ventas { get; set; }
    }
}
