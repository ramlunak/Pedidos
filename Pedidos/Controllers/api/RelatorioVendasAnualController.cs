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
    public class RelatorioVendasAnualApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RelatorioVendasAnualApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<P_RelatorioVendasAnual> Get(int idCuenta, int year)
        {
            var relatorioVendasAnual = await _context.P_RelatorioVendasAnual.Where(x => x.idCuenta == idCuenta && x.year == year).ToListAsync();
            if (relatorioVendasAnual.FirstOrDefault() == null) return default(P_RelatorioVendasAnual);
            return relatorioVendasAnual.FirstOrDefault();
        }

        [HttpPost]
        public async Task<string> Post([FromBody] P_RelatorioVendasAnual relatorioVendasAnual)
        {
            if (relatorioVendasAnual is null) return false.ToString();

            try
            {
                var exist = await _context.P_RelatorioVendasAnual.Where(x => x.idCuenta == relatorioVendasAnual.idCuenta && x.year == relatorioVendasAnual.year).ToListAsync();
                if (exist.Any())
                {
                    var relatorioDb = exist.FirstOrDefault();
                    relatorioDb.idCuenta = relatorioVendasAnual.idCuenta;
                    relatorioDb.year = relatorioVendasAnual.year;
                    relatorioDb.enero = relatorioVendasAnual.enero;
                    relatorioDb.febrero = relatorioVendasAnual.febrero;
                    relatorioDb.marzo = relatorioVendasAnual.marzo;
                    relatorioDb.abril = relatorioVendasAnual.abril;
                    relatorioDb.mayo = relatorioVendasAnual.mayo;
                    relatorioDb.junio = relatorioVendasAnual.junio;
                    relatorioDb.julio = relatorioVendasAnual.julio;
                    relatorioDb.agosto = relatorioVendasAnual.agosto;
                    relatorioDb.septiembre = relatorioVendasAnual.septiembre;
                    relatorioDb.octubre = relatorioVendasAnual.octubre;
                    relatorioDb.noviembre = relatorioVendasAnual.noviembre;
                    relatorioDb.diciembre = relatorioVendasAnual.diciembre;

                    _context.P_RelatorioVendasAnual.Update(relatorioDb);
                    await _context.SaveChangesAsync();
                    return true.ToString();
                }
                else
                {
                    _context.Add(relatorioVendasAnual);
                    await _context.SaveChangesAsync();
                    return true.ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

    }
}
