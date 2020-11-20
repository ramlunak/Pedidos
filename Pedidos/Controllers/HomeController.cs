using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspNetCore.Reporting;
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
            ValidarCuenta();
            return View();
        }

        public IActionResult Print()
        {

            var dt = new DataTable();
            dt = GetData();

            string minetype = "";
            int extension = 1;

            var path = $"{this._webHostEnvironment.WebRootPath}\\Reports\\Vendas.rdlc";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("Data", "Rdl Report");
            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("ds", dt);           
           // localReport.AddDataSource("ds", new List<P_Cuenta> { new P_Cuenta { id = 1 } });
            var result = localReport.Execute(RenderType.Pdf, extension, parameters, minetype);           
            return File(result.MainStream, "application/pdf");

        }


        public DataTable GetData()
        {
            var dtsds = new DataTable();
            dtsds.Columns.Add("id");
            dtsds.Columns.Add("nome");
            DataRow row = dtsds.NewRow();
            row["id"] = "1";
            row["nome"] = "1";
            dtsds.Rows.Add(row);
            return dtsds;
        }

        public IActionResult Privacy()
        {
            ValidarCuenta();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            ValidarCuenta();
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
