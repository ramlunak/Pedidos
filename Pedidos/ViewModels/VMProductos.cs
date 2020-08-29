using Pedidos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.ViewModels
{
    public class VMProductos : BaseModelo
    {
        public List<P_Productos> Productos { get; set; }
    }
}
