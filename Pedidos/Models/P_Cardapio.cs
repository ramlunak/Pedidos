using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Models
{
    public class P_Cardapio
    {
        public int id { get; set; }
        public int idProducto { get; set; }

        [Required(ErrorMessage = "O valor é obrigatorio")]
        [DisplayName("Valor")]
        [DisplayFormat(DataFormatString = "{0:0,0000}")]
        [DataType(DataType.Currency)]
        public decimal valor { get; set; }   
        
        public DateTime fecha { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "O produto é obrigatorio")]
        [DisplayName("Tempo de preparação")]
        public int? minutosPreparacion { get; set; }
        public int? horasPreparacion { get; set; }
        public int idCuenta { get; set; }
        public bool activo { get; set; } = true;

        //AUXILIARES
        [Required(ErrorMessage = "O produto é obrigatorio")]
        [DisplayName("Produto")]
        [NotMapped]
        public string Producto { get; set; }
                
    }
}
