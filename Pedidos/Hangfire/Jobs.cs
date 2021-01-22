using Microsoft.EntityFrameworkCore;
using Pedidos.Controllers;
using Pedidos.Data;
using Pedidos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos.Hangfire
{
    public class Jobs : AppDbContext
    {
        public Jobs()
        {

        }

        public async Task RelatorioVendasSaidasAnual()
        {
            try
            {
                // var cuentas = await _context.P_Cuentas.ToListAsync();
            }
            catch (Exception ex)
            {
                ;
            }

            ;
        }
    }
}
