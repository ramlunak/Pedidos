using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Models
{
    public class P_Cliente
    {
        public int id { get; set; }

        [DisplayName("Telefone")]
        public string telefono { get; set; }

        [Required(ErrorMessage = "O nome é obrigatorio")]
        [DisplayName("Nome")]
        public string nombre { get; set; }
        public int idCuenta { get; set; }
        public bool activo { get; set; } = true;
    }
}
