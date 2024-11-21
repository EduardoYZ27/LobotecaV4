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
    public class SancionesController : Controller
    {
        private readonly LobotecaContext _context;

        public SancionesController(LobotecaContext context)
        {
            _context = context;
        }

        // GET: Sanciones
        public async Task<IActionResult> Index()
        {
            var lobotecaContext = _context.Sanciones.Include(s => s.IdDevolucionesNavigation).Include(s => s.IdPrestamoNavigation).Include(s => s.IdUsuarioNavigation);
            return View(await lobotecaContext.ToListAsync());
        }

        // GET: Sanciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Sanciones == null)
            {
                return NotFound();
            }

            var sancione = await _context.Sanciones
                .Include(s => s.IdDevolucionesNavigation)
                .Include(s => s.IdPrestamoNavigation)
                .Include(s => s.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sancione == null)
            {
                return NotFound();
            }

            return View(sancione);
        }

        // GET: Sanciones/Create
        public IActionResult Create()
        {
            ViewData["IdDevoluciones"] = new SelectList(_context.Devoluciones, "Id", "Id");
            ViewData["IdPrestamo"] = new SelectList(_context.Prestamos, "Id", "Id");
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Id");
            return View();
        }

        // POST: Sanciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdUsuario,IdPrestamo,IdDevoluciones,Monto")] Sancione sancione)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sancione);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdDevoluciones"] = new SelectList(_context.Devoluciones, "Id", "Id", sancione.IdDevoluciones);
            ViewData["IdPrestamo"] = new SelectList(_context.Prestamos, "Id", "Id", sancione.IdPrestamo);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Id", sancione.IdUsuario);
            return View(sancione);
        }

        // GET: Sanciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Sanciones == null)
            {
                return NotFound();
            }

            var sancione = await _context.Sanciones.FindAsync(id);
            if (sancione == null)
            {
                return NotFound();
            }
            ViewData["IdDevoluciones"] = new SelectList(_context.Devoluciones, "Id", "Id", sancione.IdDevoluciones);
            ViewData["IdPrestamo"] = new SelectList(_context.Prestamos, "Id", "Id", sancione.IdPrestamo);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Id", sancione.IdUsuario);
            return View(sancione);
        }

        // POST: Sanciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdUsuario,IdPrestamo,IdDevoluciones,Monto")] Sancione sancione)
        {
            if (id != sancione.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sancione);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SancioneExists(sancione.Id))
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
            ViewData["IdDevoluciones"] = new SelectList(_context.Devoluciones, "Id", "Id", sancione.IdDevoluciones);
            ViewData["IdPrestamo"] = new SelectList(_context.Prestamos, "Id", "Id", sancione.IdPrestamo);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Id", sancione.IdUsuario);
            return View(sancione);
        }

        // GET: Sanciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Sanciones == null)
            {
                return NotFound();
            }

            var sancione = await _context.Sanciones
                .Include(s => s.IdDevolucionesNavigation)
                .Include(s => s.IdPrestamoNavigation)
                .Include(s => s.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sancione == null)
            {
                return NotFound();
            }

            return View(sancione);
        }

        // POST: Sanciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sanciones == null)
            {
                return Problem("Entity set 'LobotecaContext.Sanciones'  is null.");
            }
            var sancione = await _context.Sanciones.FindAsync(id);
            if (sancione != null)
            {
                _context.Sanciones.Remove(sancione);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SancioneExists(int id)
        {
          return (_context.Sanciones?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
