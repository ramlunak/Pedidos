using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pedidos.Data;
using Pedidos.Models;
using Pedidos.Models.DTO;

namespace Pedidos.Controllers
{
    public class AdicionaisController : BaseController
    {
        private readonly AppDbContext _context;

        public AdicionaisController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _context.P_Adicionais.Where(x => x.idCuenta == Cuenta.id).ToListAsync();
            return View(model.OrderByDescending(x => x.paraTodos).ToList());
        }

        public async Task<IActionResult> Categorias(int id)
        {
            var query = await _context.P_Categorias.FromSqlRaw($"EXEC GetCategoriasPorAdicional @idAdicional = '{id}',@idCuenta = '{Cuenta.id}'").ToListAsync();
            var model = from CT in query
                        select new ListarCategoriasPorAdicional()
                        {
                            idAdicional = id,
                            idCategoria = CT.id,
                            categoria = CT.nombre,
                            selected = CT.activo
                        };

            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var p_Adicionais = await _context.P_Adicionais
                .FirstOrDefaultAsync(m => m.id == id);
            if (p_Adicionais == null)
            {
                return NotFound();
            }

            return View(p_Adicionais);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(P_Adicionais p_Adicionais)
        {
            ValidarCuenta();
            if (ModelState.IsValid)
            {
                if (ExistsByName(p_Adicionais.nombre))
                {
                    NotifyError("Já existe um adicional com esse nome.");
                    return View(p_Adicionais);
                }

                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        p_Adicionais.idCuenta = Cuenta.id;
                        p_Adicionais.activo = true;
                        _context.Add(p_Adicionais);
                        await _context.SaveChangesAsync();

                        var p_AdicionalCategorias = new P_AdicionalCategorias()
                        {
                            idAdicional = p_Adicionais.id,
                            idCuenta = Cuenta.id
                        };

                        _context.Add(p_AdicionalCategorias);
                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        NotifyError(ex.Message);
                    }
                }


                return RedirectToAction(nameof(Index));
            }
            return View(p_Adicionais);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var p_Adicionais = await _context.P_Adicionais.FindAsync(id);
            if (p_Adicionais == null)
            {
                return NotFound();
            }
            return View(p_Adicionais);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, P_Adicionais p_Adicionais)
        {
            if (id != p_Adicionais.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(p_Adicionais);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!P_AdicionaisExists(p_Adicionais.id))
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
            return View(p_Adicionais);
        }

        public async Task<IActionResult> ChangeStatus(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var p_Adicionais = await _context.P_Adicionais.FirstOrDefaultAsync(m => m.id == id);
            if (p_Adicionais == null)
            {
                return NotFound();
            }

            try
            {
                p_Adicionais.activo = !p_Adicionais.activo;
                _context.Update(p_Adicionais);
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
            if (id == null)
            {
                return NotFound();
            }

            var p_Adicionais = await _context.P_Adicionais.FirstOrDefaultAsync(m => m.id == id);
            if (p_Adicionais == null)
            {
                return NotFound();
            }

            try
            {
                p_Adicionais.paraTodos = !p_Adicionais.paraTodos;
                _context.Update(p_Adicionais);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return NotFound();
            }

            return Ok(true);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategoriaInAdicionalCategorias([FromBody]ListarCategoriasPorAdicional listarCategoriasPorAdicional)
        {
            int result;

            result = await _context.Database.ExecuteSqlRawAsync($"EXEC InsertIfNotExistAdicionalCategorias @idAdicional = {listarCategoriasPorAdicional.idAdicional},@idCuenta = {Cuenta.id}");

            if (listarCategoriasPorAdicional.selected)
            {               
                result = await _context.Database.ExecuteSqlRawAsync($"EXEC AddCategoriaInAdicionalCategorias @idAdicional = {listarCategoriasPorAdicional.idAdicional},  @idCategoria = {listarCategoriasPorAdicional.idCategoria},@idCuenta = {Cuenta.id}");
            }
            else
            {
                result = await _context.Database.ExecuteSqlRawAsync($"EXEC DeleteCategoriaInAdicionalCategorias @idAdicional = {listarCategoriasPorAdicional.idAdicional},  @idCategoria = {listarCategoriasPorAdicional.idCategoria},@idCuenta = {Cuenta.id}");
                //  await _context.Database.ExecuteSqlRawAsync($"EXEC DeleteCategoriaAdicional @idAdicional = {listarCategoriasPorAdicional.idAdicional},  @idCategoria = {listarCategoriasPorAdicional.idCategoria},@idCuenta = {Cuenta.id}");
            }


            if (result == 0)
            {
                return NotFound();
            }

            return Ok(true);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var p_Adicionais = await _context.P_Adicionais
                .FirstOrDefaultAsync(m => m.id == id);
            if (p_Adicionais == null)
            {
                return NotFound();
            }

            return View(p_Adicionais);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var p_Adicionais = await _context.P_Adicionais.FindAsync(id);
            _context.P_Adicionais.Remove(p_Adicionais);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool P_AdicionaisExists(int id)
        {
            return _context.P_Adicionais.Any(e => e.id == id && e.idCuenta == Cuenta.id);
        }


        private bool ExistsByName(string nombre)
        {
            return _context.P_Adicionais.Any(e => e.nombre == nombre && e.idCuenta == Cuenta.id);
        }
    }
}
