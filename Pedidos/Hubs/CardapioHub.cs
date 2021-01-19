using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
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

        public string ServerGetConnectionId() => Context.ConnectionId;

        public async Task ServerAbrirMesa(string idCuenta, string valor)
        {
            var clientes = await _context.P_Clientes.Where(x => x.idCuenta.ToString() == idCuenta).ToListAsync();
            await Clients.All.SendAsync("clientAbrirMesa", idCuenta, clientes.ToJson());
        }


    }
}
