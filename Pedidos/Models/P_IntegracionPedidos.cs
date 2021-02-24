using Pedidos.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Pedidos.Extensions;

namespace Pedidos.Models
{
    public class P_IntegracionPedidos
    {
        public int id { get; set; }
        public int idPedido { get; set; }
        public int idCuenta { get; set; }
        public int idCuentaIntegracion { get; set; }
        public string statusIntegracion { get; set; }
        public DateTime fecha { get; set; }
        public DateTime? fechaEnviado { get; set; }
        public DateTime? fechaEntregado { get; set; }
    }
}
