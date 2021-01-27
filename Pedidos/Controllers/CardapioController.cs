using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pedidos.Data;
using Pedidos.Extensions;
using Pedidos.Models;

namespace Pedidos.Controllers
{

    public class CardapioController : BaseController
    {
        private readonly AppDbContext _context;

        public CardapioController(AppDbContext context)
        {
            _context = context;
        }

        [Route("Cardapio/{id}")]
        public async Task<IActionResult> Index(string id)
        {

            if (id == null) return View();
            TempData["IsQRCode"] = true;

            var cuenta = id.Split("_")[0];
            var table = id.Split("_")[1];
            var idCuenta = Convert.ToInt32(cuenta.Split("acc")[1]);
            var mesa = Convert.ToInt32(table.Split("table")[1]);
            ViewBag.IdCuenta = idCuenta;
            ViewBag.Mesa = mesa;

            return View();
        }

        public async Task<IActionResult> CargarCategorias(int id)
        {

            var productos = await _context.P_Productos.Where(x => x.idCuenta == id && x.activo).ToListAsync();
            SetSession("CardapioProductos", productos);

            try
            {
                TempData["IsQRCode"] = true;

                var idCuenta = id;
                var model = await _context.P_Categorias.Where(x => x.idCuenta == idCuenta && x.activo).ToListAsync();

                return Ok(model);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetProductos([FromBody] P_Categoria categoria)
        {
            var productos = GetSession<List<P_Productos>>("CardapioProductos");

            if (productos == null)
            {
                productos = await _context.P_Productos.Where(x => x.idCuenta == categoria.idCuenta && x.activo).ToListAsync();
                SetSession("CardapioProductos", productos);
            }

            var items = productos.Where(x => x.idCategoria == categoria.id && x.idCuenta == categoria.idCuenta && x.activo).ToList();
            return Ok(items);
        }

        public async Task<IActionResult> VerificarCliente()
        {
            return Ok(Cliente);
        }

        public async Task<IActionResult> CadastrarCliente(int idCuenta, string nombre)
        {
            var cliente = new P_Cliente();
            cliente.idCuenta = idCuenta;
            cliente.nombre = nombre;
            cliente.registroPorCardapio = true;

            try
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                await GuardarClienteCookie(cliente);
                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return NotFound(new { ok = false });
            }
        }

        public async Task GuardarClienteCookie(P_Cliente cliente)
        {
            Logof();
            try
            {
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim("clienteCardapio", JsonConvert.SerializeObject(cliente)));
                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                var authProperties = new AuthenticationProperties
                {
                    //AllowRefresh = <bool>,
                    // Refreshing the authentication session should be allowed.

                    //ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1),

                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.

                    IsPersistent = true,
                    // Whether the authentication session is persisted across 
                    // multiple requests. When used with cookies, controls
                    // whether the cookie's lifetime is absolute (matching the
                    // lifetime of the authentication ticket) or session-based.

                    //IssuedUtc = <DateTimeOffset>,
                    // The time at which the authentication ticket was issued.

                    //RedirectUri = <string>
                    // The full path or absolute URI to be used as an http 
                    // redirect response value.
                };

                await HttpContext.SignInAsync(principal, authProperties);
            }
            catch (Exception)
            {

            }
        }

        public P_Cliente Cliente
        {
            get
            {
                var cliente = new P_Cliente();
                string json = null;
                try
                {
                    json = User.Claims.First(x => x.Type == "clienteCardapio").Value;
                }
                catch
                {

                }

                if (json != null)
                {
                    return JsonConvert.DeserializeObject<P_Cliente>(json);
                }
                else
                {
                    return null;
                }

            }
        }


    }
}
