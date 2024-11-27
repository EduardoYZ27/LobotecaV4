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
            var prestamos = await _context.Prestamos
                .Include(p => p.IdAdministradorNavigation)
                .Include(p => p.IdLibroNavigation)
                .Include(p => p.IdUsuarioNavigation)
                .ToListAsync();

            // Verifica si cada préstamo tiene una devolución asociada
            foreach (var prestamo in prestamos)
            {
                prestamo.TieneDevolucion = _context.Devoluciones.Any(d => d.IdPrestamo == prestamo.Id);
            }

            return View(prestamos);
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
            ViewData["IdAdministrador"] = new SelectList(_context.Administradors, "Id", "Nombre");
            ViewData["IdLibro"] = new SelectList(_context.Libros, "Id", "Titulo");
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Nombre");
            return View();
        }

        // POST: Prestamo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdAdministrador,IdLibro,IdUsuario")] Prestamo prestamo)
        {
            if (ModelState.IsValid)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    // Asignar fecha actual como fecha de préstamo
                    prestamo.FechaDePrestamo = DateTime.Now;

                    // Calcular la fecha de término (3 días hábiles)
                    DateTime fechaDeTermino = prestamo.FechaDePrestamo;
                    int diasHabiles = 0;
                    while (diasHabiles < 3)
                    {
                        fechaDeTermino = fechaDeTermino.AddDays(1);
                        if (fechaDeTermino.DayOfWeek != DayOfWeek.Saturday && fechaDeTermino.DayOfWeek != DayOfWeek.Sunday)
                        {
                            diasHabiles++;
                        }
                    }
                    prestamo.FechaDeTermino = fechaDeTermino;

                    // Buscar el inventario del libro
                    var inventarioLibro = await _context.InventarioLibros
                        .Include(il => il.IdInventarioNavigation)
                        .FirstOrDefaultAsync(il => il.IdLibro == prestamo.IdLibro);

                    if (inventarioLibro == null || inventarioLibro.IdInventarioNavigation == null)
                    {
                        ModelState.AddModelError("", "No se encontró inventario para este libro o está mal configurado.");
                        return ReloadCreateView(prestamo);
                    }

                    // Validar la cantidad disponible
                    if (inventarioLibro.IdInventarioNavigation.CantidadSistema <= 0)
                    {
                        ModelState.AddModelError("", "No hay ejemplares disponibles.");
                        return ReloadCreateView(prestamo);
                    }

                    // Reducir cantidad en el sistema
                    inventarioLibro.IdInventarioNavigation.CantidadSistema -= 1;
                    _context.Inventarios.Update(inventarioLibro.IdInventarioNavigation);

                    // Registrar el préstamo
                    _context.Add(prestamo);
                    await _context.SaveChangesAsync();

                    // Confirmar la transacción
                    await transaction.CommitAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    ModelState.AddModelError("", $"Error al registrar el préstamo: {ex.Message}");
                }
            }

            return ReloadCreateView(prestamo);
        }

        // Método auxiliar para recargar las listas desplegables
        private IActionResult ReloadCreateView(Prestamo prestamo)
        {
            ViewData["IdAdministrador"] = new SelectList(_context.Administradors, "Id", "Nombre", prestamo.IdAdministrador);
            ViewData["IdLibro"] = new SelectList(_context.Libros, "Id", "Titulo", prestamo.IdLibro);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Nombre", prestamo.IdUsuario);
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
            ViewData["IdAdministrador"] = new SelectList(_context.Administradors, "Id", "Nombre", prestamo.IdAdministrador);
            ViewData["IdLibro"] = new SelectList(_context.Libros, "Id", "Titulo", prestamo.IdLibro);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Nombre", prestamo.IdUsuario);
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

        // POST: Prestamo/Devolucion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Devolver(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Buscar el préstamo correspondiente
                var prestamo = await _context.Prestamos
                    .Include(p => p.IdLibroNavigation)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (prestamo == null)
                {
                    ModelState.AddModelError("", "No se encontró el préstamo especificado.");
                    return RedirectToAction(nameof(Index));
                }

                // Buscar el inventario asociado al libro
                var inventarioLibro = await _context.InventarioLibros
                    .Include(il => il.IdInventarioNavigation)
                    .FirstOrDefaultAsync(il => il.IdLibro == prestamo.IdLibro);

                if (inventarioLibro == null || inventarioLibro.IdInventarioNavigation == null)
                {
                    ModelState.AddModelError("", "No se encontró el inventario asociado al libro.");
                    return RedirectToAction(nameof(Index));
                }

                // Incrementar la cantidad en el sistema
                inventarioLibro.IdInventarioNavigation.CantidadSistema += 1;

                // Actualizar la entidad en el contexto
                _context.Update(inventarioLibro.IdInventarioNavigation);

                // Crear la devolución
                var devolucion = new Devolucione
                {
                    IdPrestamo = prestamo.Id,
                    IdUsuario = prestamo.IdUsuario, // Asociar el usuario que hizo el préstamo
                    IdAdministrador = 1, // Reemplaza con el ID del administrador actual
                    Fecha = DateTime.Now,
                    Condiciones = "Devolución completada automáticamente."
                };
                _context.Devoluciones.Add(devolucion);

                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError("", $"Error al procesar la devolución: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

    }
}
