using Pedidos.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Models
{
    public class P_Sabor
    {
        public int id { get; set; }
        [Required]
        public string nombre { get; set; }
        public decimal? valor { get; set; }
        public int idCuenta { get; set; }
        public bool activo { get; set; }
        //[NotMapped]
        //public string text
        //{
        //    get
        //    {
        //        return $"{nombre},{(valor.HasValue ? $" R$ {valor.Value.ToString("F2")}" : "")}";
        //    }
        //}

    }
}
