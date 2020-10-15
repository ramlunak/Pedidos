using Pedidos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.ViewModels
{
    public class VMAplicativos : BaseModelo
    {
        public List<P_Aplicativo> Aplicativos { get; set; }
    }
}
