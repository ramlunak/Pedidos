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

        public async Task ChatSendMessage(string connectionId, string message)
        {
            // await Clients.Client(connectionId).SendAsync("receivedMessage", message);
            await Clients.All.SendAsync("receivedMessage", message);
        }

    }
}
