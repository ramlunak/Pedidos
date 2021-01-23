using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pedidos.Data;
using Pedidos.Models;
using Pedidos.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pedidos.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PedidosApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<P_Pedido>> Get(int idCuenta, int mes, int year)
        {
            var pedidos = await _context.P_Pedidos.Where(x => x.idCuenta == idCuenta && x.status == StatusPedido.Finalizado.ToString() && x.fecha.Month == mes && x.fecha.Year == year).ToListAsync();
            if (pedidos == null) return default(List<P_Pedido>);
            return pedidos;
        }

    }
}
