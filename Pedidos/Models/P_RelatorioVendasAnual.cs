using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Models
{
    public class P_RelatorioVendasAnual
    {
        public int id { get; set; }
        public int idCuenta { get; set; }
        public int year { get; set; }
        public decimal enero { get; set; } = 0;
        public decimal febrero { get; set; } = 0;
        public decimal marzo { get; set; } = 0;
        public decimal abril { get; set; } = 0;
        public decimal mayo { get; set; } = 0;
        public decimal junio { get; set; } = 0;
        public decimal julio { get; set; } = 0;
        public decimal agosto { get; set; } = 0;
        public decimal septiembre { get; set; } = 0;
        public decimal octubre { get; set; } = 0;
        public decimal noviembre { get; set; } = 0;
        public decimal diciembre { get; set; } = 0;
    }
}
