using Pedidos.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Models
{
    public class P_Barrio
    {
        public int id { get; set; }
        [Required(ErrorMessage = "O Estado é obrigatorio")]
        [DisplayName("Estado")]
        public string estado { get; set; }
        [Required(ErrorMessage = "O Municipio é obrigatorio")]
        [DisplayName("Municipio")]
        public string municipio { get; set; }
        [Required(ErrorMessage = "O Barrio é obrigatorio")]
        [DisplayName("Nome")]
        public string nombre { get; set; }
        public decimal? tasa { get; set; }
        public int idCuenta { get; set; }
        public bool activo { get; set; }
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
