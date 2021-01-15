using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Models
{
    public class P_Mejorias
    {
        public int id { get; set; }
        public string codigo { get; set; }
        [Required(ErrorMessage = "O mensagem é obrigatorio")]
        [DisplayName("Mensagem")]
        public string texto { get; set; }
        public int idCuenta { get; set; }
        public string status { get; set; }
    }
}
