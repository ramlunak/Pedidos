using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pedidos.Models;
using Pedidos.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Pedidos.Controllers
{
    public class BaseController : Controller
    {
        public async Task SignIn(P_Cuenta cuenta)
        {
            Logof();
            try
            {
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim("cuenta", JsonConvert.SerializeObject(cuenta)));
                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                var authProperties = new AuthenticationProperties
                {
                    //AllowRefresh = <bool>,
                    // Refreshing the authentication session should be allowed.

                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1),
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

        public void Logof()
        {
            try
            {
                Response.Cookies.Delete("Pedidos");
                HttpContext.Session.Clear();
            }
            catch (Exception)
            {

            }
        }

        public void ValidarCuenta()
        {
            if (Cuenta is null || !Cuenta.activo)
            {
                Logof();
                Response.Redirect("/Login");
            }
            try
            {
                TempData["Email"] = Cuenta.usuario;
            }
            catch (Exception ex)
            {
                Response.Redirect("/Login");
            }
        }

        public void SetSession(string key, string value)
        {
            HttpContext.Session.SetString(key, value);
        }

        public void SetSession(string key, object o)
        {
            var json = JsonConvert.SerializeObject(o);
            HttpContext.Session.SetString(key, json);
        }

        public string GetSession(string key)
        {
            return string.IsNullOrEmpty(HttpContext.Session.GetString(key)) ? null : HttpContext.Session.GetString(key);
        }

        public T GetSession<T>(string key)
        {
            var value = string.IsNullOrEmpty(HttpContext.Session.GetString(key)) ? null : HttpContext.Session.GetString(key);
            if (value is null) return default(T);
            return JsonConvert.DeserializeObject<T>(value);
        }

        public void UpdateSessionCliente(SessionTransaction action, P_Cliente obj)
        {
            var json = HttpContext.Session.GetString("Clientes");
            if (json is null) return;

            var list = JsonConvert.DeserializeObject<List<P_Cliente>>(json);

            if (action == SessionTransaction.Add)
            {
                list.Add(obj);
            }
            else if (action == SessionTransaction.Edit && list.Any())
            {
                var old = list.First(x => x.id == obj.id);
                if (old != null)
                {
                    list.Remove(old);
                    list.Add(obj);
                }
            }
            else if (action == SessionTransaction.Delete)
            {
                list.Remove(obj);
            }

            SetSession("Clientes", list);
        }

        public void UpdateSessionAplicativos(SessionTransaction action, P_Aplicativo obj)
        {
            var json = HttpContext.Session.GetString("Aplicativos");
            if (json is null) return;

            var list = JsonConvert.DeserializeObject<List<P_Aplicativo>>(json);

            if (action == SessionTransaction.Add)
            {
                list.Add(obj);
            }
            else if (action == SessionTransaction.Edit && list.Any())
            {
                var old = list.First(x => x.id == obj.id);
                if (old != null)
                {
                    list.Remove(old);
                    list.Add(obj);
                }
            }
            else if (action == SessionTransaction.Delete)
            {
                list.Remove(obj);
            }

            SetSession("Aplicativos", list);
        }

        public void UpdateSessionProductos(SessionTransaction action, P_Productos obj)
        {
            var json = HttpContext.Session.GetString("Productos");
            if (json is null) return;

            var list = JsonConvert.DeserializeObject<List<P_Productos>>(json);

            if (action == SessionTransaction.Add)
            {
                list.Add(obj);
            }
            else if (action == SessionTransaction.Edit && list.Any())
            {
                var old = list.First(x => x.id == obj.id);
                if (old != null)
                {
                    list.Remove(old);
                    list.Add(obj);
                }
            }
            else if (action == SessionTransaction.Delete)
            {
                list.Remove(obj);
            }

            SetSession("Aplicativos", list);
        }

        public P_Cuenta Cuenta
        {
            get
            {
                var cuenta = new P_Cuenta();
                string json = null;
                try
                {
                    json = User.Claims.First(x => x.Type == "cuenta").Value;
                }
                catch
                {

                }

                if (json != null)
                {
                    return JsonConvert.DeserializeObject<P_Cuenta>(json);
                }
                else
                {
                    return null;
                }

            }
        }

        public void NotifySuccess(string message)
        {
            var msg = new
            {
                message = message,
                icon = NotificationType.success.ToString(),
                type = NotificationType.success.ToString(),
                provider = "sweetAlert"
            };

            TempData["Message"] = JsonConvert.SerializeObject(msg);
        }

        public void NotifyError(string message)
        {
            var msg = new
            {
                message = message,
                icon = NotificationType.error.ToString(),
                type = NotificationType.error.ToString(),
                provider = "sweetAlert"
            };

            TempData["Message"] = JsonConvert.SerializeObject(msg);
        }

        public void Notifywarning(string message)
        {
            var msg = new
            {
                message = message,
                icon = NotificationType.warning.ToString(),
                type = NotificationType.warning.ToString(),
                provider = "sweetAlert"
            };

            TempData["Message"] = JsonConvert.SerializeObject(msg);
        }

    }
}
