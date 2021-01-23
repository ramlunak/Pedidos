using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pedidos.Controllers;
using Pedidos.Data;
using Pedidos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pedidos.Hangfire
{
    public class Jobs : AppDbContext
    {

        public Jobs()
        {

        }

        static async Task<List<P_Cuenta>> GetCuentas()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync("https://devmastereat.azurewebsites.net/api/cuentas");
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<P_Cuenta>>(responseBody);
                }
                catch (HttpRequestException e)
                {
                    return default(List<P_Cuenta>);
                }
            }
        }

        static async Task<List<P_Cuenta>> GetPedidosByIdCuenta()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync("https://devmastereat.azurewebsites.net/api/cuentas");
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<P_Cuenta>>(responseBody);
                }
                catch (HttpRequestException e)
                {
                    return default(List<P_Cuenta>);
                }
            }
        }

        public async Task RelatorioVendasSaidasAnual()
        {
            var cuentas = await GetCuentas();

            if (cuentas != null)
                foreach (var item in cuentas)
                {




                }
            ;
        }
    }
}
