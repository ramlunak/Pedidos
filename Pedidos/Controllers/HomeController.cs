using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pedidos.Models;

namespace Pedidos.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
            this._webHostEnvironment = webHostEnvironment;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        public IActionResult Index()
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            return View();
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
    }
}
