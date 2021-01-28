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
using System.Threading.Tasks;

namespace Pedidos.Hubs
{
    public class Message
    {
        public int idCliente { get; set; }
        public int idCuenta { get; set; }
        public int mesa { get; set; }
        public string titulo { get; set; }
        public string message { get; set; }
        public string position { get; set; }
        public string color { get; set; }
        public string margin { get; set; }
        public bool send { get; set; }
    }


    public class CardapioHub : Hub
    {
        private readonly AppDbContext _context;

        public CardapioHub(AppDbContext context)
        {
            _context = context;
        }

        public string GetConnectionId() => Context.ConnectionId;

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var isCardapio = httpContext.Request.Query["isCardapio"];

            if (isCardapio.Count == 0)
            {
                var json = Context.User.Claims.First(x => x.Type == "cuenta").Value;
                if (!string.IsNullOrEmpty(json))
                {
                    var cuenta = JsonConvert.DeserializeObject<P_Cuenta>(json);
                    if (cuenta != null)
                    {
                        await Groups.AddToGroupAsync(Context.ConnectionId, "Cuenta" + cuenta.id);
                    }
                }
            }

            await base.OnConnectedAsync();
        }

        public async Task clienteSendMessage(string connectionId, string idCuenta, string mesa, string message)
        {
            // await Clients.Client(connectionId).SendAsync("receivedMessage", message);

            var serverMensaje = new Message
            {
                idCliente = 1,
                idCuenta = 1,
                mesa = 1,
                titulo = "Royber | Mesa 1",
                message = "Resivido",
                position = "float-left",
                color = "bg-info",
                margin = "mr-5",
                send = false
            };

            await Clients.Client(connectionId).SendAsync("clienteReceivedMessage", serverMensaje.ToJson());
        }

    }
}
