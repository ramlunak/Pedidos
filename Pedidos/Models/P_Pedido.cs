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
        public decimal? valor { get; set; }
        public decimal? descuento { get; set; }

        //AUXILIARES

        [NotMapped]      
        public int? IdProducto { get; set; }
        [Required(ErrorMessage = "O Produco é obrigatorio")]
        [NotMapped]
        public string Producto { get; set; }
        [NotMapped]
        public int Cantidad { get; set; }
        [NotMapped]
        public string Observacion { get; set; }
        [NotMapped]
        public string IdsProducto { get; set; }
        [NotMapped]
        public List<P_Productos> Productos = new List<P_Productos>();
        [NotMapped]
        public string ValorTotal
        {
            get
            {
                this.valor = Productos.Sum(x => x.valor);
                return this.valor.Value.ToString("C");
            }
        }

    }
}
