﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Pedidos.Data;
using Pedidos.Models;

namespace Pedidos.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int pagina = 1)
        {          
            var cantidadRegistrosPorPagina = 5; // parámetro

            var temas = await _context.P_Categorias.OrderBy(x => x.id)
                   .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                   .Take(cantidadRegistrosPorPagina).ToListAsync();

            var totalDeRegistros = await _context.P_Categorias.CountAsync();

            var modelo = new ViewModels.VMCategorias();
            modelo.Categorias = temas;
            modelo.PaginaActual = pagina;
            modelo.TotalDeRegistros = totalDeRegistros;
            modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
            modelo.ValoresQueryString = new RouteValueDictionary();
            modelo.ValoresQueryString["pagina"] = pagina;

            return View(modelo);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(P_Categoria p_Categoria)
        {
            if (ModelState.IsValid)
            {
                p_Categoria.idCuenta = 1;

                _context.Add(p_Categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(p_Categoria);
        }

        public async Task<IActionResult> Edit(int? id, int? pagina)
        {
            if (id == null)
            {
                return NotFound();
            }

            var p_Categoria = await _context.P_Categorias.FindAsync(id);
            if (p_Categoria == null)
            {
                return NotFound();
            }

            ViewBag.Pagina = pagina;
            return View(p_Categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, P_Categoria p_Categoria,int? pagina)
        {
            if (id != p_Categoria.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(p_Categoria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!P_CategoriaExists(p_Categoria.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index),new{pagina});
            }
            return View(p_Categoria);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var p_Categoria = await _context.P_Categorias
                .FirstOrDefaultAsync(m => m.id == id);
            if (p_Categoria == null)
            {
                return NotFound();
            }

            return View(p_Categoria);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var p_Categoria = await _context.P_Categorias.FindAsync(id);
            _context.P_Categorias.Remove(p_Categoria);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool P_CategoriaExists(int id)
        {
            return _context.P_Categorias.Any(e => e.id == id);
        }
    }
}
