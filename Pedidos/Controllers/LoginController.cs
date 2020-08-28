using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pedidos.Data;
using Pedidos.Models;

namespace Pedidos.Controllers
{
    public class LoginController : BaseController
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        // GET: P_Cuenta
        public async Task<IActionResult> Index()
        {
            if (Cuenta != null)
            {
                return RedirectToAction(nameof(Index), "Home", null);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(P_Cuenta login)
        {
            try
            {
                var cuenta = await _context.P_Cuentas.Where(x => x.usuario == login.usuario && x.password == login.password).FirstOrDefaultAsync();
                if (cuenta is null)
                {
                    ViewBag.Erro = "Usuario não cadastrado";
                }
                else if (!cuenta.activo)
                {
                    ViewBag.Erro = "Sua conta foi desativada, contate o suporte técnico.";
                }
                else
                {
                    await SignIn(cuenta);
                    return RedirectToAction(nameof(Index), "Home", null);
                }
            }
            catch
            {
                ViewBag.Erro = "Erro de conexão, contate o suporte técnico.";
            }

            return View(login);
        }

        // GET: P_Cuenta/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var p_Cuenta = await _context.P_Cuentas.FindAsync(id);
            if (p_Cuenta == null)
            {
                return NotFound();
            }
            return View(p_Cuenta);
        }

        // POST: P_Cuenta/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,usuario,password,logged,idPlano,activo")] P_Cuenta p_Cuenta)
        {
            if (id != p_Cuenta.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(p_Cuenta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!P_CuentaExists(p_Cuenta.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(p_Cuenta);
        }

        // GET: P_Cuenta/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var p_Cuenta = await _context.P_Cuentas
                .FirstOrDefaultAsync(m => m.id == id);
            if (p_Cuenta == null)
            {
                return NotFound();
            }

            return View(p_Cuenta);
        }

        // POST: P_Cuenta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var p_Cuenta = await _context.P_Cuentas.FindAsync(id);
            _context.P_Cuentas.Remove(p_Cuenta);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool P_CuentaExists(int id)
        {
            return _context.P_Cuentas.Any(e => e.id == id);
        }
    }
}
