using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Models.DTO
{
    public class FiltroReporteDePedido
    {
        public DateTime? fechaInicio { get; set; }
        public DateTime? fechaFin { get; set; }
    }
}
