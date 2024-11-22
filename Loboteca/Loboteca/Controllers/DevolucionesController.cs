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
    public class DevolucionesController : Controller
    {
        private readonly LobotecaContext _context;

        public DevolucionesController(LobotecaContext context)
        {
            _context = context;
        }

        // GET: Devoluciones
        public async Task<IActionResult> Index()
        {
            var lobotecaContext = _context.Devoluciones.Include(d => d.IdAdministradorNavigation).Include(d => d.IdPrestamoNavigation).Include(d => d.IdUsuarioNavigation);
            return View(await lobotecaContext.ToListAsync());
        }

        // GET: Devoluciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Devoluciones == null)
            {
                return NotFound();
            }

            var devolucione = await _context.Devoluciones
                .Include(d => d.IdAdministradorNavigation)
                .Include(d => d.IdPrestamoNavigation)
                .Include(d => d.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (devolucione == null)
            {
                return NotFound();
            }

            return View(devolucione);
        }

        // GET: Devoluciones/Create
        public IActionResult Create()
        {
            ViewData["IdAdministrador"] = new SelectList(_context.Administradors, "Id", "Nombre");
            ViewData["IdPrestamo"] = new SelectList(_context.Prestamos, "Id", "Id");
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Nombre");
            return View();
        }

        // POST: Devoluciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdAdministrador,IdUsuario,IdPrestamo,Fecha,Condiciones")] Devolucione devolucione)
        {
            if (ModelState.IsValid)
            {
                _context.Add(devolucione);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAdministrador"] = new SelectList(_context.Administradors, "Id", "Nombre", devolucione.IdAdministrador);
            ViewData["IdPrestamo"] = new SelectList(_context.Prestamos, "Id", "Id", devolucione.IdPrestamo);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Nombre", devolucione.IdUsuario);
            return View(devolucione);
        }

        // GET: Devoluciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Devoluciones == null)
            {
                return NotFound();
            }

            var devolucione = await _context.Devoluciones.FindAsync(id);
            if (devolucione == null)
            {
                return NotFound();
            }
            ViewData["IdAdministrador"] = new SelectList(_context.Administradors, "Id", "Nombre", devolucione.IdAdministrador);
            ViewData["IdPrestamo"] = new SelectList(_context.Prestamos, "Id", "Id", devolucione.IdPrestamo);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Nombre", devolucione.IdUsuario);
            return View(devolucione);
        }

        // POST: Devoluciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdAdministrador,IdUsuario,IdPrestamo,Fecha,Condiciones")] Devolucione devolucione)
        {
            if (id != devolucione.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(devolucione);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DevolucioneExists(devolucione.Id))
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
            ViewData["IdAdministrador"] = new SelectList(_context.Administradors, "Id", "Nombre", devolucione.IdAdministrador);
            ViewData["IdPrestamo"] = new SelectList(_context.Prestamos, "Id", "Id", devolucione.IdPrestamo);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Nombre", devolucione.IdUsuario);
            return View(devolucione);
        }

        // GET: Devoluciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Devoluciones == null)
            {
                return NotFound();
            }

            var devolucione = await _context.Devoluciones
                .Include(d => d.IdAdministradorNavigation)
                .Include(d => d.IdPrestamoNavigation)
                .Include(d => d.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (devolucione == null)
            {
                return NotFound();
            }

            return View(devolucione);
        }

        // POST: Devoluciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Devoluciones == null)
            {
                return Problem("Entity set 'LobotecaContext.Devoluciones'  is null.");
            }
            var devolucione = await _context.Devoluciones.FindAsync(id);
            if (devolucione != null)
            {
                _context.Devoluciones.Remove(devolucione);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DevolucioneExists(int id)
        {
          return (_context.Devoluciones?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
