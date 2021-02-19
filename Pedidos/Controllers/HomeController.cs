using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pedidos.Data;
using Pedidos.Extensions;
using Pedidos.Models;
using Pedidos.Models.Enums;

namespace Pedidos.Controllers
{
    public class HomeController : BaseController
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }

            ViewBag.CuentaIntegracion = Cuenta.rol == RolesSistema.Integracion.ToString() ? true : false;

            var model = await _context.P_Pedidos.Where(x => x.codigo == "P1-141").ToListAsync();
            var pedido = model.FirstOrDefault();
            pedido.productos = pedido.jsonListProductos.ConvertTo<List<P_Productos>>();

            return View(pedido);
        }

        public IActionResult Print()
        {

            return View();

            //var path = $"{this._webHostEnvironment.WebRootPath}\\Reports\\Vendas.rdlc";

            //try
            //{
            //    //var dt = new DataTable();
            //    //dt = GetData();

            //    string minetype = "x";
            //    int extension = 1;

            //    Dictionary<string, string> parameters = new Dictionary<string, string>();
            //    parameters.Add("Data", "Rdl Report");

            //    LocalReport localReport = new LocalReport(path);
            //    var data = new List<P_Categoria>(){
            //    new P_Categoria { id = 1,nombre = "sadas"},
            //    new P_Categoria { id = 2,nombre = "sadas"}
            //    };

            //    localReport.AddDataSource("ds", data);
            //    // localReport.AddDataSource("ds", new List<P_Cuenta> { new P_Cuenta { id = 1 } });
            //    var result = localReport.Execute(RenderType.Pdf, extension, null, minetype);
            //    return File(result.MainStream, "application/pdf");
            //}
            //catch (Exception ex)
            //{
            //    ViewBag.Erro = ex.Message.ToString();
            //    ViewBag.Path = path;
            //    return View();
            //}

        }

        public DataTable GetData()
        {

            var dtsds = new DataTable();
            dtsds.Columns.Add("id");
            dtsds.Columns.Add("nome");
            DataRow row = dtsds.NewRow();
            row["id"] = 1;
            row["nome"] = "1";
            dtsds.Rows.Add(row);
            return dtsds;
        }

        public IActionResult Privacy()
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

         [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult AccessDenied()
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public async Task<IActionResult> GetReporteVentasAnual()
        {

            var year = DateTime.Now.ToSouthAmericaStandard().Year;
            var reportes = await _context.P_RelatorioVendasAnual.Where(x => x.idCuenta == Cuenta.id && x.year == year).ToListAsync();
            decimal[] array = new decimal[12];

            if (reportes.Any())
            {
                var reporte = reportes.FirstOrDefault();
                array[0] = reporte.enero;
                array[1] = reporte.febrero;
                array[2] = reporte.marzo;
                array[3] = reporte.abril;
                array[4] = reporte.mayo;
                array[5] = reporte.junio;
                array[6] = reporte.julio;
                array[7] = reporte.agosto;
                array[8] = reporte.septiembre;
                array[9] = reporte.octubre;
                array[10] = reporte.noviembre;
                array[11] = reporte.diciembre;
                return Ok(array);
            }
            else
            {
                return Ok(array);
            }

        }

    }
}
