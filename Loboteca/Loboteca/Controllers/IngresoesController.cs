using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Loboteca.Models;

namespace Loboteca.Controllers
{
    public class IngresoesController : Controller
    {
        private readonly LobotecaContext _context;

        public IngresoesController(LobotecaContext context)
        {
            _context = context;
        }

        // GET: Ingresoes
        public async Task<IActionResult> Index()
        {
            var lobotecaContext = _context.Ingresos.Include(i => i.IdLibroNavigation);
            return View(await lobotecaContext.ToListAsync());
        }

        // GET: Ingresoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Ingresos == null)
            {
                return NotFound();
            }

            var ingreso = await _context.Ingresos
                .Include(i => i.IdLibroNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ingreso == null)
            {
                return NotFound();
            }

            return View(ingreso);
        }

        // GET: Ingresoes/Create
        public IActionResult Create()
        {
            ViewData["IdLibro"] = new SelectList(_context.Libros, "Id", "Titulo");
            return View();
        }

        // POST: Ingresoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdLibro,FechaIngreso,Ejemplares")] Ingreso ingreso)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ingreso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdLibro"] = new SelectList(_context.Libros, "Id", "Titulo", ingreso.IdLibro);
            return View(ingreso);
        }

        // GET: Ingresoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Ingresos == null)
            {
                return NotFound();
            }

            var ingreso = await _context.Ingresos.FindAsync(id);
            if (ingreso == null)
            {
                return NotFound();
            }
            ViewData["IdLibro"] = new SelectList(_context.Libros, "Id", "Titulo", ingreso.IdLibro);
            return View(ingreso);
        }

        // POST: Ingresoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdLibro,FechaIngreso,Ejemplares")] Ingreso ingreso)
        {
            if (id != ingreso.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ingreso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IngresoExists(ingreso.Id))
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
            ViewData["IdLibro"] = new SelectList(_context.Libros, "Id", "Titulo", ingreso.IdLibro);
            return View(ingreso);
        }

        // GET: Ingresoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Ingresos == null)
            {
                return NotFound();
            }

            var ingreso = await _context.Ingresos
                .Include(i => i.IdLibroNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ingreso == null)
            {
                return NotFound();
            }

            return View(ingreso);
        }

        // POST: Ingresoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Ingresos == null)
            {
                return Problem("Entity set 'LobotecaContext.Ingresos'  is null.");
            }
            var ingreso = await _context.Ingresos.FindAsync(id);
            if (ingreso != null)
            {
                _context.Ingresos.Remove(ingreso);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IngresoExists(int id)
        {
          return (_context.Ingresos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
