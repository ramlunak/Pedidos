using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pedidos.Data;
using Pedidos.Extensions;
using Pedidos.Models;
using Pedidos.Models.DTO;

namespace Pedidos.Controllers
{
    public class SaboresController : BaseController
    {
        private readonly AppDbContext _context;

        public SaboresController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }

            var model = await _context.P_Sabores.Where(x => x.idCuenta == Cuenta.id).ToListAsync();
            return View(model.OrderBy(x => x.nombre));
        }

        public async Task<IActionResult> Create()
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(P_Sabor p_Sabor)
        {
            if (!ValidarCuenta())
            {
                Response.Redirect("/Login");
            }

            if (ModelState.IsValid)
            {
                //if (await ExistsByName(p_Sabor.nombre))
                //{
                //    NotifyError("Já existe um adicional com esse nome.");
                //    return View(p_Sabor);
                //}

                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        p_Sabor.idCuenta = Cuenta.id;
                        p_Sabor.activo = true;

                        _context.Add(p_Sabor);
                        await _context.SaveChangesAsync();

                        var p_AdicionalCategorias = new P_AdicionalCategorias()
                        {
                            idAdicional = p_Sabor.id,
                            idCuenta = Cuenta.id
                        };

                        _context.Add(p_AdicionalCategorias);
                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();

                        await InsertLog(_context, Cuenta.id, ex.ToString());
                        PrompErro(ex.Message);
                        return View(p_Sabor);
                    }
                }

                return View(p_Sabor);

            }
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }

            if (id == null)
            {
                return NotFound();
            }

            var p_Sabor = await _context.P_Sabores.FindAsync(id);
            if (p_Sabor == null)
            {
                return NotFound();
            }
            return View(p_Sabor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, P_Sabor p_Sabor)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }

            if (id != p_Sabor.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //var idAdicional = await GetIdByName(p_Sabor.nombre);
                //if (idAdicional != null && idAdicional != p_Sabor.id)
                //{
                //    PrompInfo("O adicional já existe");
                //    return View(p_Sabor);
                //}

                try
                {
                    var entidad = await _context.P_Sabores.FindAsync(id);
                    entidad.nombre = p_Sabor.nombre;
                    entidad.valor = p_Sabor.valor;

                    _context.Update(entidad);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!P_SaboresExists(p_Sabor.id))
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
            return View(p_Sabor);
        }

        public async Task<IActionResult> ChangeStatus(int? id)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            if (id == null)
            {
                return NotFound();
            }

            var p_Sabor = await _context.P_Sabores.FirstOrDefaultAsync(m => m.id == id);
            if (p_Sabor == null)
            {
                return NotFound();
            }

            try
            {
                p_Sabor.activo = !p_Sabor.activo;
                _context.Update(p_Sabor);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return NotFound();
            }

            return Ok(true);
        }

        public async Task<IActionResult> ChangeVsibilidad(int? id)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            if (id == null)
            {
                return NotFound();
            }

            var p_Sabor = await _context.P_Sabores.FirstOrDefaultAsync(m => m.id == id);
            if (p_Sabor == null)
            {
                return NotFound();
            }

            try
            {
                _context.Update(p_Sabor);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return NotFound();
            }

            return Ok(true);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            if (id == null)
            {
                return NotFound();
            }

            var p_Sabor = await _context.P_Sabores
                .FirstOrDefaultAsync(m => m.id == id);
            if (p_Sabor == null)
            {
                return NotFound();
            }

            return View(p_Sabor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!ValidarCuenta())
            {
                return RedirectToAction("Salir", "Login");
            }
            var p_Sabor = await _context.P_Sabores.FindAsync(id);
            _context.P_Sabores.Remove(p_Sabor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool P_SaboresExists(int id)
        {

            return _context.P_Sabores.Any(e => e.id == id && e.idCuenta == Cuenta.id);
        }

        public async Task<IActionResult> EditarOrden(int? id, string orden)
        {

            try
            {
                var p_Sabor = await _context.P_Sabores.FindAsync(id);
                _context.Update(p_Sabor);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return NotFound(ex.ToString());
            }
            return Ok(true);
        }

        private async Task<int?> GetIdByName(string nombre)
        {
            try
            {
                var adiacional = await _context.P_Sabores.FirstOrDefaultAsync(e => e.nombre.ToLower() == nombre.ToLower() && e.idCuenta == Cuenta.id);
                if (adiacional == null)
                {
                    return null;
                }
                return adiacional.id;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private async Task<bool> ExistsByName(string nombre)
        {
            return await _context.P_Sabores.AnyAsync(e => e.nombre == nombre && e.idCuenta == Cuenta.id);
        }
    }
}
