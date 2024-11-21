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
    public class PrestamoController : Controller
    {
        private readonly LobotecaContext _context;

        public PrestamoController(LobotecaContext context)
        {
            _context = context;
        }

        // GET: Prestamo
        public async Task<IActionResult> Index()
        {
            var lobotecaContext = _context.Prestamos.Include(p => p.IdAdministradorNavigation).Include(p => p.IdLibroNavigation).Include(p => p.IdUsuarioNavigation);
            return View(await lobotecaContext.ToListAsync());
        }

        // GET: Prestamo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Prestamos == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamos
                .Include(p => p.IdAdministradorNavigation)
                .Include(p => p.IdLibroNavigation)
                .Include(p => p.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prestamo == null)
            {
                return NotFound();
            }

            return View(prestamo);
        }

        // GET: Prestamo/Create
        public IActionResult Create()
        {
            ViewData["IdAdministrador"] = new SelectList(_context.Administradors, "Id", "Id");
            ViewData["IdLibro"] = new SelectList(_context.Libros, "Id", "Id");
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Id");
            return View();
        }

        // POST: Prestamo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FechaDePrestamo,FechaDeTermino,IdAdministrador,IdLibro,IdUsuario")] Prestamo prestamo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(prestamo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAdministrador"] = new SelectList(_context.Administradors, "Id", "Id", prestamo.IdAdministrador);
            ViewData["IdLibro"] = new SelectList(_context.Libros, "Id", "Id", prestamo.IdLibro);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Id", prestamo.IdUsuario);
            return View(prestamo);
        }

        // GET: Prestamo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Prestamos == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo == null)
            {
                return NotFound();
            }
            ViewData["IdAdministrador"] = new SelectList(_context.Administradors, "Id", "Id", prestamo.IdAdministrador);
            ViewData["IdLibro"] = new SelectList(_context.Libros, "Id", "Id", prestamo.IdLibro);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Id", prestamo.IdUsuario);
            return View(prestamo);
        }

        // POST: Prestamo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FechaDePrestamo,FechaDeTermino,IdAdministrador,IdLibro,IdUsuario")] Prestamo prestamo)
        {
            if (id != prestamo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prestamo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrestamoExists(prestamo.Id))
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
            ViewData["IdAdministrador"] = new SelectList(_context.Administradors, "Id", "Id", prestamo.IdAdministrador);
            ViewData["IdLibro"] = new SelectList(_context.Libros, "Id", "Id", prestamo.IdLibro);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Id", prestamo.IdUsuario);
            return View(prestamo);
        }

        // GET: Prestamo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Prestamos == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamos
                .Include(p => p.IdAdministradorNavigation)
                .Include(p => p.IdLibroNavigation)
                .Include(p => p.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prestamo == null)
            {
                return NotFound();
            }

            return View(prestamo);
        }

        // POST: Prestamo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Prestamos == null)
            {
                return Problem("Entity set 'LobotecaContext.Prestamos'  is null.");
            }
            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo != null)
            {
                _context.Prestamos.Remove(prestamo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrestamoExists(int id)
        {
          return (_context.Prestamos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
