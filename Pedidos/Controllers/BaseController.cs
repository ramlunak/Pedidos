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
        public const string ImageLoad = "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";
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
