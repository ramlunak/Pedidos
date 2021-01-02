﻿using Newtonsoft.Json;
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
            this.codigo = Utils.Util.CreateCodigoPedido(idCuenta);
            this.date_teste = DateTime.Now.ToSouthAmericaStandard().ToString();
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
        public string status { get; set; }
        public string descripcion { get; set; }
        public decimal? descuento { get; set; }
        public string jsonListProductos { get; set; }
        public string telefono { get; set; }
        public decimal total { get; set; } = 0;
        public int idCuenta { get; set; }
        public bool activo { get; set; }
        public bool pago { get; set; } = false;
        public string jsonFormaPagamento { get; set; }

        //AUXILIARES

        [NotMapped]
        public bool isNew { get; set; } = true;

        [NotMapped]
        public string date_teste { get; set; }

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
        public decimal valorProductos
        {
            get
            {
                var valorProductos = productos.Sum(x => x.ValorMasAdicionales);
                return valorProductos;
            }
        }
        [NotMapped]
        public List<P_FormaPagamento> listaFormaPagamento { get; set; } = new List<P_FormaPagamento>();

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
        public bool pago { get; set; }
        public string listaFormaPagamento { get; set; }
    }

}