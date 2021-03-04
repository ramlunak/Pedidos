using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Models
{
    public class P_Categoria
    {
      
        public int id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [DisplayName("Nome")]
        public string nombre { get; set; }  
                
        [DisplayName("Código")]
        public string codigo { get; set; }  
         
        public int idCuenta { get; set; }

        [DisplayName("Ativo")]
        public bool activo { get; set; } = true;

        [NotMapped]
        public List<P_Aux> P_AuxList { get; set; }
    }
}
