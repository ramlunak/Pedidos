using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Pedidos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Hubs
{
    public class CardapioHub : Hub
    {

        public string ServerGetConnectionId() => Context.ConnectionId;

        public async Task ServerAbrirMesa(string idCuenta, string valor)
        {
            await Clients.All.SendAsync("clientAbrirMesa", idCuenta, valor);
        }


    }
}
