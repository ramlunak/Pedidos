using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Models
{
    public class P_Cuenta
    {
        public int id { get; set; }
        public string usuario { get; set; }
        public string password { get; set; }
        public string rol { get; set; }
        public int? idPlano { get; set; }
        public bool activo { get; set; } = true;

        //Configuraciones
        public string estado { get; set; }
        public string municipio { get; set; }

    }
}
