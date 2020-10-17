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

        //AUXILIARES
        [NotMapped]
        public List<P_Productos> Productos { get; set; } = new List<P_Productos>();

        [NotMapped]
        public List<P_Cliente> Clientes { get; set; } = new List<P_Cliente>();

        [NotMapped]
        public List<P_Aplicativo> Aplicativos { get; set; } = new List<P_Aplicativo>();
    }
}
