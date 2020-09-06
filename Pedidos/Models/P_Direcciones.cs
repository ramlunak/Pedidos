using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Models
{
    public class P_Direcciones
    {
        public int id { get; set; }
              
        [DisplayName("CEP")]
        public string code { get; set; }

        [Required(ErrorMessage = "O Estado é obrigatorio")]
        [MaxLength(2)]
        [DisplayName("Estado")]
        public string state { get; set; }

        [Required(ErrorMessage = "O Municipio é obrigatorio")]
        [DisplayName("Municipio")]
        public string city { get; set; }

        [Required(ErrorMessage = "O Barrio é obrigatorio")]
        [DisplayName("Barrio")]
        public string district { get; set; }

        [Required(ErrorMessage = "A Rua é obrigatorio")]
        [DisplayName("Rua")]
        public string address { get; set; }

        [Required(ErrorMessage = "O Número é obrigatorio")]
        [DisplayName("Número")]
        public string numero { get; set; }
        public string complemento { get; set; }
        
        public int idCliente { get; set; }
        public int idPedido { get; set; }
        public int idCuenta { get; set; }
        public bool activo { get; set; } = true;
    }
}
