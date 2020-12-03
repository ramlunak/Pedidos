using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Models
{
    public class P_Ingredientes
    {
        public int id { get; set; }      
        [Required(ErrorMessage = "O nome é obrigatorio")]
        [DisplayName("Nome")]
        public string nombre { get; set; }
        public int idCuenta { get; set; }
        public bool activo { get; set; } = true;

        [NotMapped]
        public bool selected { get; set; } = true;
    }
}
