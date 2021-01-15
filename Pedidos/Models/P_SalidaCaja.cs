using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Models
{
    public class P_SalidaCaja
    {
        public int id { get; set; }
        [Required(ErrorMessage = "O motivo é obrigatorio")]
        [DisplayName("Motivo")]
        public int idMotivo { get; set; }
        [Required(ErrorMessage = "O valor é obrigatorio")]
        [DisplayName("Valor")]
        public decimal valor { get; set; }
        public int? idCaja { get; set; }
        public int idCuenta { get; set; }
        public DateTime fecha { get; set; }
        public bool activo { get; set; }

        [NotMapped]
        public string motivo { get; set; }
    }
}
