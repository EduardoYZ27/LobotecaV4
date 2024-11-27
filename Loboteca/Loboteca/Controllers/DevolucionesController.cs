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
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    // Registrar la devolución
                    _context.Add(devolucione);

                    // Buscar el préstamo asociado
                    var prestamo = await _context.Prestamos
                        .Include(p => p.IdLibroNavigation)
                        .FirstOrDefaultAsync(p => p.Id == devolucione.IdPrestamo);

                    if (prestamo == null)
                    {
                        ModelState.AddModelError("", "No se encontró el préstamo asociado.");
                        return View(devolucione);
                    }

                    // Buscar el inventario asociado al libro
                    var inventarioLibro = await _context.InventarioLibros
                        .Include(il => il.IdInventarioNavigation)
                        .FirstOrDefaultAsync(il => il.IdLibro == prestamo.IdLibro);

                    if (inventarioLibro == null)
                    {
                        ModelState.AddModelError("", "No se encontró inventario para este libro.");
                        return View(devolucione);
                    }

                    // Verificar que el inventario esté correctamente configurado
                    if (inventarioLibro.IdInventarioNavigation == null)
                    {
                        ModelState.AddModelError("", "El inventario asociado al libro no está configurado correctamente.");
                        return View(devolucione);
                    }

                    // Incrementar la cantidad en el sistema
                    inventarioLibro.IdInventarioNavigation.CantidadSistema += 1;

                    // Actualizar el inventario en el contexto
                    _context.Update(inventarioLibro.IdInventarioNavigation);

                    // Guardar los cambios y confirmar la transacción
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    ModelState.AddModelError("", $"Error al registrar la devolución: {ex.Message}");
                }
            }

            // Recargar ViewBag en caso de error
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
