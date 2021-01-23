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
using Pedidos.Extensions;

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
                    HttpResponseMessage response = await client.GetAsync("https://localhost:44335/api/cuentasapi");
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

        static async Task<List<P_Pedido>> GetPedidosByIdCuenta(int idCuenta, int mes, int year)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var uri = $"localhost:44335/api/pedidosapi?idCuenta={idCuenta}&mes={mes}&year={year}";
                    HttpResponseMessage response = await client.GetAsync("https://" + uri);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<P_Pedido>>(responseBody);
                }
                catch (HttpRequestException e)
                {
                    return default(List<P_Pedido>);
                }
            }
        }

        public async Task RelatorioVendasSaidasAnual()
        {
            var cuentas = await GetCuentas();

            if (cuentas != null)
                foreach (var item in cuentas)
                {
                    var pedidos_mes = await GetPedidosByIdCuenta(item.id, DateTime.Now.ToSouthAmericaStandard().Month, DateTime.Now.ToSouthAmericaStandard().Year);
                    ;

                }
            ;
        }
    }
}
