using Pedidos.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Models
{
    public class P_Caja
    {
        public int id { get; set; }
        public string status { get; set; }
        public DateTime fecha { get; set; }
        public DateTime dataInicio { get; set; }
        public DateTime dataFin { get; set; }
        public decimal totalVentas { get; set; }
        public decimal totalTasas { get; set; }

        [NotMapped]
        public List<P_FormaPagamento> formaPagamentos { get; set; } = new List<P_FormaPagamento>();

        public int idCuenta { get; set; }
        public bool activo { get; set; } = true;
    }
}
