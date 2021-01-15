using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Models
{
    public class P_MotivoSalidaCaja
    {
        public int id { get; set; }

        [Required(ErrorMessage = "O motivo é obrigatorio")]
        [DisplayName("Motivo")]
        public string motivo { get; set; }
        public int idCuenta { get; set; }
        [DisplayName("Ativo")]
        public bool activo { get; set; } = true;
    }
}
