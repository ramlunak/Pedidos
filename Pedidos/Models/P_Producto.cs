using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Models
{
    public class P_Productos
    {
        public int id { get; set; }
        public string codigo { get; set; }

        [Required(ErrorMessage = "O nome é obrigatorio")]
        [DisplayName("Nome")]
        public string nombre { get; set; }

        [Column(TypeName = "image")]
        public byte[] imagen { get; set; }

        [NotMapped]
        public IFormFile ImageName { get; set; }
        [NotMapped]
        public string ImageBase64 { get; set; }


        [Required(ErrorMessage = "A Categoria é obrigatoria")]
        [DisplayName("Categoria")]
        public int idCategoria { get; set; }
        [NotMapped]
        public string Categoria { get; set; }
        public int idCuenta { get; set; }
        public bool activo { get; set; }
    }
}
