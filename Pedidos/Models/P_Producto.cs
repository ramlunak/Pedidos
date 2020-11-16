﻿using Microsoft.AspNetCore.Http;
using Pedidos.Models.Enums;
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

        [DisplayName("descrição")]
        public string descripcion { get; set; }

        [Required(ErrorMessage = "A Categoria é obrigatoria")]
        [DisplayName("Categoria")]
        public int idCategoria { get; set; }
        public int? idSubCategoria { get; set; }


        [Column(TypeName = "image")]
        public byte[] imagen { get; set; }

        public int idCuenta { get; set; }

        [DisplayName("Valor")]
        [DataType(DataType.Currency)]
        public decimal valor { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "O valor é obrigatorio")]
        [DisplayName("Valor")]
        public string strValor { get; set; } = string.Empty;

        [DisplayName("U/M")]
        public UnidadMedida? unidadeMedida { get; set; }

        public int? horasPreparacion { get; set; }
        public int? minutosPreparacion { get; set; }

        [DisplayName("Ativo")]
        public bool activo { get; set; } = true;

        //PROPIEDADES AUXILIARES
       
        [NotMapped]
        [Required(ErrorMessage = "A Categoria é obrigatoria")]
        [DisplayName("Categoria")]
        public string Categoria { get; set; }

        [NotMapped]
        public IFormFile ImageName { get; set; }
        [NotMapped]
        public string ImageBase64 { get; set; }

        //PARA LOS PEDIDOS
        [NotMapped]
        public DateTime DataPedido { get; set; }
        [NotMapped]
        public string Observacion { get; set; }
        [NotMapped]
        public int Index { get; set; }

    }
}
