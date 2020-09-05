using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Models
{
    public class P_SubCategoria
    {
        public int id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatorio")]
        [DisplayName("Nome")]
        public string nombre { get; set; }

        [Key, ForeignKey("P_Categoria")]
        public int idCategotia { get; set; }
        public int idCuenta { get; set; }

        [DisplayName("Ativo")]
        public bool activo { get; set; } = true;
    }
}
