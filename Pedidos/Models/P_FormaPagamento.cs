using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Models
{
    public class P_FormaPagamento
    {
        public int id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatorio")]
        [DisplayName("Nome")]
        public string nombre { get; set; }
        public int? idAplicativo { get; set; }
        public int idCuenta { get; set; }

        [DisplayName("Ativo")]
        public bool activo { get; set; } = true;

        public bool app { get; set; } = false;

        public decimal? tasa { get; set; }

        //Para Guardar Por Pedido
        [NotMapped]
        public decimal valor { get; set; }
        [NotMapped]
        public decimal valorTasa { get; set; }

    }
}
