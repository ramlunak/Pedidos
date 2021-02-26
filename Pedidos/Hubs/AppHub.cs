using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pedidos.Data;
using Pedidos.Extensions;
using Pedidos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Pedidos.Hubs
{
    public class MessageHub
    {
        public string chatConnectionId { get; set; }
        public string codigoConeccionCliente { get; set; }
        public string idCuenta { get; set; }
        public string mesa { get; set; }
        public string nombreCliente { get; set; }
        public string message { get; set; }
        public string position { get; set; }
        public string color { get; set; }
        public string margin { get; set; }
        public bool clientSend { get; set; }
        public bool cuentaSend { get; set; }
    }

    public class GroupMessageHub
    {
        public string codigoConeccionCliente { get; set; }
        public string nombreCliente { get; set; }
        public string mesa { get; set; }
        public int sinLeer { get; set; }
        public List<MessageHub> mensajes { get; set; }
    }

    public class AppHub : Hub
    {
        private readonly AppDbContext _context;

        public AppHub(AppDbContext context)
        {
            _context = context;
        }

        public string GetConnectionId() => Context.ConnectionId;

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var isCardapio = httpContext.Request.Query["isCardapio"];
            var codigoConecionCliente = httpContext.Request.Query["codigo_coneccion_cliente"];

            if (isCardapio.Count > 0)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, codigoConecionCliente[0]);
            }

            await base.OnConnectedAsync();
        }

        #region CLIENTE SEND

        public async Task clienteSendMessage(string connectionId, string idCuenta, string mesa, string message)
        {
            await Clients.User($"mastereat_account_{idCuenta}").SendAsync("serverReceivedMessage", message);
        }

        public async Task clienteSendProducto(string connectionId, string idCuenta, string mesa, string producto)
        {
            await Clients.User($"mastereat_account_{idCuenta}").SendAsync("serverReceivedProducto", producto);
        }

        #endregion

        #region ESTABLECIMIENTO SEND 

        public async Task establecimientoSendMessage(string codigoConeccionCliente, string message)
        {
            await Clients.Group(codigoConeccionCliente).SendAsync("clienteReceivedMessage", message);
        }

        #endregion

        #region

        public async Task enviarPedidoIntegracion(int idCuentaIntegracion, string pedido)
        {
            await Clients.User($"mastereat_account_{idCuentaIntegracion}").SendAsync("integracionRecibirPedido", pedido);
        }
        
        public async Task cancelarPedidoIntegracion(int idCuentaIntegracion, string pedido)
        {
            await Clients.User($"mastereat_account_{idCuentaIntegracion}").SendAsync("integracionCancelarPedido", pedido);
        }

        #endregion
    }
}
