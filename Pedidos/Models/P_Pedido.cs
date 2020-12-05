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
        public P_Pedido(int idCuenta)
        {
            this.idCuenta = idCuenta;
        }

        public int id { get; set; }
        public string codigo
        {
            get
            {
                var cuenta = this.idCuenta.ToString();
                var year = DateTime.Now.Year.ToString();
                var mes = DateTime.Now.Month.ToString();
                var dia = DateTime.Now.Day.ToString();
                var hora = DateTime.Now.Hour.ToString();
                var min = DateTime.Now.Minute.ToString();
                var sec = DateTime.Now.Second.ToString();

                return "P" + cuenta + year + mes + dia + hora + min + sec;
            }
        }
        public int? idCliente { get; set; }
        public int? idDireccion { get; set; }
        public int? idMesa { get; set; }
        public int? idAplicativo { get; set; }
        public DateTime fecha { get; set; }
        public string status { get; set; }
        public string descripcion { get; set; }
        public decimal? valor { get; set; }
        public decimal? descuento { get; set; }
        public string JsonProductosDTO { get; set; }
        public int idCuenta { get; set; }
        public bool activo { get; set; }

        //AUXILIARES

        [NotMapped]
        public List<P_Productos> productos { get; set; } = new List<P_Productos>();
        [NotMapped]
        public string cliente { get; set; }
        [NotMapped]
        public string direccion { get; set; }
        [NotMapped]
        public string telefono { get; set; }

        //[Required(ErrorMessage = "O Produco é obrigatorio")]
        //[NotMapped]
        //public string Producto { get; set; }
        //[NotMapped]
        //public int Cantidad { get; set; }
        //[NotMapped]
        //public string Observacion { get; set; }
        //[NotMapped]
        //public PedidoDTO PedidoDTO { get; set; }
        //[NotMapped]
        //public List<P_Productos> Productos = new List<P_Productos>();
        //[NotMapped]
        //public string ValorTotal
        //{
        //    get
        //    {
        //        this.valor = Productos.Sum(x => x.valor);
        //        return this.valor.Value.ToString("C");
        //    }
        //}

    }


    //public class PedidoDTO
    //{
    //    public int? IdCliente { get; set; }
    //    public string cliente { get; set; }
    //    public string telefono { get; set; }

    //    public int? idMesa { get; set; }

    //    public int? IdAplicativo { get; set; }
    //    public string aplicativo { get; set; }

    //    public int? idDireccion { get; set; }
    //    public string code { get; set; }
    //    public string state { get; set; }
    //    public string city { get; set; }
    //    public string district { get; set; }
    //    public string address { get; set; }
    //    public string numero { get; set; }
    //    public string complemento { get; set; }

    //}

}