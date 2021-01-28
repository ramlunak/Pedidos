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

        public string ServerGetConnectionId() => Context.ConnectionId;

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

        public async Task client_abrirMesa(string connectionId, string idCuenta, string valor)
        {

            await Clients.Client(connectionId).SendAsync("server_aprovarMesa", idCuenta, null);
        }

        public async Task client_AddProducto(string connectionId, string idCuenta, string valor)
        {


            await Clients.Client(connectionId).SendAsync("server_aprovarMesa", idCuenta, null);
        }




    }
}
