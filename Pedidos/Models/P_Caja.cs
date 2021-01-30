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
    public class P_Caja
    {
        public int id { get; set; }
        public int? idPrimerPedido { get; set; }
        public int idUltimoPedido { get; set; }
        public DateTime fecha { get; set; } = DateTime.Now.ToSouthAmericaStandard();
        [Required(ErrorMessage = "O Valor inicial é obrigatorio")]
        [DisplayName("Valor inicial")]
        public decimal inicio { get; set; }
        public decimal? totalVentas { get; set; }
        public decimal? totalTasas { get; set; }
        public decimal? totalDescuentos { get; set; }
        public decimal? totalTasasEntrega { get; set; }
        public string jsonFormaPagamento { get; set; }
        public int idCuenta { get; set; }
        public bool isOpen { get; set; }

        [NotMapped]
        public List<P_FormaPagamento> formaPagamentos { get; set; } = new List<P_FormaPagamento>();

    }
}
