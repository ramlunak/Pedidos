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
    public class P_IntegracionRuta
    {
        public int id { get; set; }
        public int[] idsPedido { get; set; }
        public DTOIntegracionBarrio[] barrios { get; set; }
        public int idCuentaIntegracion { get; set; }
        public int idEntregador { get; set; }
        public string entregador { get; set; }
        public string statusRuta { get; set; }
        public DateTime fecha { get; set; }
        public DateTime? fechaEnviado { get; set; }
        public DateTime? fechaEntregado { get; set; }
    }

    public class DTOIntegracionBarrio
    {
        public string nombre { get; set; }
        public int count { get; set; }
        public string usuario { get; set; }
    }

}