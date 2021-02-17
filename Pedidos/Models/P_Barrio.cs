﻿using Pedidos.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Models
{
    public class P_Barrio
    {
        public int id { get; set; }
        [Required]
        public string estado { get; set; }
        [Required]
        public string municipio { get; set; }
        [Required]
        public string nombre { get; set; }
        public decimal? tasa { get; set; }
        public int idCuenta { get; set; }
        public bool activo { get; set; }
        [NotMapped]
        public string text
        {
            get
            {
                var mun = municipio.Substring(0, 4);
                return $"{nombre}, {mun}, {estado.ToUpper()}";
            }
        }

    }
}
