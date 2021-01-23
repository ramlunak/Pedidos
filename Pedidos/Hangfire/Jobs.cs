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

        static async Task UptadeRelatorioVendasAnual(P_RelatorioVendasAnual relatorioVendasAnual)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.PostAsync("https://localhost:44335/api/relatoriovendasanualapi", relatorioVendasAnual.ToStringContent());
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    //   return conve JsonConvert.DeserializeObject<List<P_Cuenta>>(responseBody);
                    ;
                }
                catch (HttpRequestException e)
                {
                    // return default(List<P_Cuenta>);
                    ;
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
                    if (pedidos_mes != null && pedidos_mes.Any())
                    {
                        var relatorioVendasAnual = new P_RelatorioVendasAnual();
                        relatorioVendasAnual.idCuenta = item.id;
                        relatorioVendasAnual.year = DateTime.Now.ToSouthAmericaStandard().Year;

                        var ventas = pedidos_mes.Sum(x => x.valorProductos + x.tasaEntrega - x.descuento);

                        if (DateTime.Now.ToSouthAmericaStandard().Month == 1)
                        {
                            relatorioVendasAnual.enero = ventas;
                        }
                        if (DateTime.Now.ToSouthAmericaStandard().Month == 2)
                        {
                            relatorioVendasAnual.febrero = ventas;
                        }
                        if (DateTime.Now.ToSouthAmericaStandard().Month == 3)
                        {
                            relatorioVendasAnual.marzo = ventas;
                        }
                        if (DateTime.Now.ToSouthAmericaStandard().Month == 4)
                        {
                            relatorioVendasAnual.abril = ventas;
                        }
                        if (DateTime.Now.ToSouthAmericaStandard().Month == 5)
                        {
                            relatorioVendasAnual.mayo = ventas;
                        }
                        if (DateTime.Now.ToSouthAmericaStandard().Month == 6)
                        {
                            relatorioVendasAnual.julio = ventas;
                        }
                        if (DateTime.Now.ToSouthAmericaStandard().Month == 7)
                        {
                            relatorioVendasAnual.julio = ventas;
                        }
                        if (DateTime.Now.ToSouthAmericaStandard().Month == 8)
                        {
                            relatorioVendasAnual.agosto = ventas;
                        }
                        if (DateTime.Now.ToSouthAmericaStandard().Month == 9)
                        {
                            relatorioVendasAnual.septiembre = ventas;
                        }
                        if (DateTime.Now.ToSouthAmericaStandard().Month == 10)
                        {
                            relatorioVendasAnual.octubre = ventas;
                        }
                        if (DateTime.Now.ToSouthAmericaStandard().Month == 11)
                        {
                            relatorioVendasAnual.noviembre = ventas;
                        }
                        if (DateTime.Now.ToSouthAmericaStandard().Month == 12)
                        {
                            relatorioVendasAnual.diciembre = ventas;
                        }

                        relatorioVendasAnual.idCuenta = item.id;
                        relatorioVendasAnual.year = DateTime.Now.ToSouthAmericaStandard().Year;
                        await UptadeRelatorioVendasAnual(relatorioVendasAnual);
                    }

                }
            ;
        }
    }
}
