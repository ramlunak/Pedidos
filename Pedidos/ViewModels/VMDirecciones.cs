using Pedidos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.ViewModels
{
    public class VMDirecciones : BaseModelo
    {
        public List<P_Direcciones> Direcciones { get; set; }
    }
}
