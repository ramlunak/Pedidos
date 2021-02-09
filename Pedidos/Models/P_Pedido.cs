using Newtonsoft.Json;
using Pedidos.Extensions;
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
        public string codigo { get; set; }
        public int? idCliente { get; set; }
        public string cliente { get; set; }
        public int? idDireccion { get; set; }
        public string direccion { get; set; }
        public int? idMesa { get; set; }
        public int? idAplicativo { get; set; }
        public string aplicativo { get; set; }
        public DateTime fecha { get; set; }
        public DateTime? fechaFinalizado { get; set; }
        public string status { get; set; }
        public string descripcion { get; set; }
        public decimal descuento { get; set; } = 0;
        public decimal tasaEntrega { get; set; } = 0;
        public string jsonListProductos { get; set; }
        public string telefono { get; set; }
        //public decimal total { get; set; } = 0;
        public int idCuenta { get; set; }
        public bool activo { get; set; }
        public string jsonFormaPagamento { get; set; }
        public string jsonFormaPagamentoAux { get; set; }
        public decimal valorProductos { get; set; } = 0;

        public decimal? deliveryTroco { get; set; }
        public decimal? deliveryDinheiroTotal { get; set; }
        public bool? deliveryEmCartao { get; set; } = true;
        public bool? deliveryPago { get; set; } = false;
        public bool? deliveryEmdinheiro { get; set; } = false;

        //AUXILIARES

        [NotMapped]
        public bool isNew { get; set; } = true;

        [NotMapped]
        public int tiempo_pedido
        {
            get
            {
                if (fecha < new DateTime(2020, 1, 1)) return 0;
                var tiempo = DateTime.Now.ToSouthAmericaStandard() - fecha;
                var segusdos = tiempo.TotalSeconds;
                return Convert.ToInt32(segusdos);
            }
        }

        [NotMapped]
        public List<P_Productos> productos { get; set; } = new List<P_Productos>();

        [NotMapped]
        public List<P_FormaPagamento> listaFormaPagamento { get; set; } = new List<P_FormaPagamento>();

        [NotMapped]
        public string _fecha
        {
            get
            {
                return fecha.ToString("MM/dd/yyyy HH:mm");
            }

        }


    }

    public class PedidoDatosAux
    {
        public int? idPedido { get; set; }
        public int? idCliente { get; set; }
        public string cliente { get; set; }
        public int? idAplicativo { get; set; }
        public string aplicativo { get; set; }
        public int? idMesa { get; set; }
        public string direccion { get; set; }
        public int? idDireccion { get; set; }
        public string telefono { get; set; }
        public int? idFormaPagamento { get; set; }
        public decimal? descuento { get; set; }
        public decimal? tasaEntrega { get; set; }
        public decimal? troco { get; set; }
        public bool pago { get; set; }
        public bool finalizar { get; set; }
        public string listaFormaPagamento { get; set; }

        public decimal? deliveryDinheiroTotal { get; set; }
        public decimal? deliveryTroco { get; set; }
        public bool? deliveryEmCartao { get; set; } = true;
        public bool? deliveryPago { get; set; } = false;
        public bool? deliveryEmdinheiro { get; set; } = false;

        public bool cadastrarCliente { get; set; } = true;
    }


    public class InfoAuxDelivery
    {
        public int idPedido { get; set; }
        public decimal? descuento { get; set; } = 0;
        public decimal? tasaEntrega { get; set; } = 0;
        public bool? DeliveryEmdinheiro { get; set; }
        public decimal? DeliveryDinheiroTotal { get; set; }
        public decimal? DeliveryTroco { get; set; }
        public bool? DeliveryEmCartao { get; set; }
        public bool? DeliveryPago { get; set; }
        public bool? pedidoIsPreparado { get; set; } = true;
    }

}