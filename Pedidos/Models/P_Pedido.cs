using Pedidos.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Models
{
    public class P_Pedido
    {
        public int id { get; set; }
        public int? idCliente { get; set; }
        public int? idMesa { get; set; }
        public int? idAplicativo { get; set; }
        public DateTime fecha { get; set; }
        public StatusPedido status { get; set; }
        public string descripcion { get; set; }      
        public int idCuenta { get; set; }
        public bool activo { get; set; }

        //AUXILIARES
        [Required(ErrorMessage = "O Produco é obrigatorio")]
        [NotMapped]
        public string Producto { get; set; }
        [NotMapped]
        public int IdProducto { get; set; }

        [NotMapped]
        public List<P_Productos> Productos = new List<P_Productos>();
    }
}
