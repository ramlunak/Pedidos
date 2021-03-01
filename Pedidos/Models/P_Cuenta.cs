using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Models
{
    public class P_Cuenta
    {
        public int id { get; set; }

        [Required(ErrorMessage = "O nome de usuario é obrigatorio")]
        [Column(TypeName = "nvarchar(255)")]
        [DisplayName("Usuario")]
        [StringLength(255)]
        public string usuario { get; set; }

        [Required(ErrorMessage = "A clave é obrigatoria")]
        [Column(TypeName = "nvarchar(max)")]
        [DisplayName("Clave")]
        public string password { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Confirme a clave por favor!")]
        [Compare("password", ErrorMessage = "A clave não coincide")]
        [DisplayName("Confirmar Clave")]
        public string confirmPassword { get; set; }

        public string rol { get; set; }
        public int? idPlano { get; set; }
        public int? idCuentaPadre { get; set; }
        public int? idCuentaIntegracion { get; set; }
        public bool activo { get; set; } = true;

        //Configuraciones
        public string estado { get; set; }
        public string municipio { get; set; }

        [NotMapped]
        public int? idFuncionario { get; set; }
    }
}
