using Pedidos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.ViewModels
{
    public class VMClientes : BaseModelo
    {
        public List<P_Cliente> Clientes { get; set; }
    }
}
