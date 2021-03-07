using Pedidos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.ViewModels
{
    public class VMCardapioOnline : BaseModelo
    {
        public int idCuenta { get; set; }
        public int linkCuenta { get; set; }
        public List<P_Categoria> _Categorias = new List<P_Categoria>();
        public List<P_Productos> _Productos = new List<P_Productos>();
    }
}
