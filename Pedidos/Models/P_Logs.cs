using Pedidos.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Models
{
    public class P_Log
    {
        public int id { get; set; }
        public int idCuenta { get; set; }
        public DateTime data { get; set; } = DateTime.Now.ToSouthAmericaStandard();
        public string ex { get; set; }      
    }
}
