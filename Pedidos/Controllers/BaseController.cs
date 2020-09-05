using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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

                    //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
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
    }
}
