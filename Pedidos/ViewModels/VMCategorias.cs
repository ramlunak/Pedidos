using Pedidos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.ViewModels
{
    public class VMCategorias : BaseModelo
    {
        public List<P_Categoria> Categorias { get; set; }
    }
}
