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
    public class InventarioLibroesController : Controller
    {
        private readonly LobotecaContext _context;

        public InventarioLibroesController(LobotecaContext context)
        {
            _context = context;
        }

        // GET: InventarioLibroes
        public async Task<IActionResult> Index()
        {
            var inventarioLibros = await _context.InventarioLibros
                .Include(il => il.IdInventarioNavigation)
                .Include(il => il.IdLibroNavigation)
                .ToListAsync();

            foreach (var item in inventarioLibros)
            {
                if (item.IdLibro != null)
                {
                    // Sumar los ingresos asociados al libro
                    var totalIngresos = await _context.Ingresos
                        .Where(i => i.IdLibro == item.IdLibro)
                        .SumAsync(i => i.Ejemplares);

                    // Actualizar la cantidad en sistema
                    if (item.IdInventarioNavigation != null)
                    {
                        item.IdInventarioNavigation.CantidadSistema = totalIngresos;
                    }
                }
            }

            return View(inventarioLibros);
        }

        // GET: InventarioLibroes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.InventarioLibros == null)
            {
                return NotFound();
            }

            var inventarioLibro = await _context.InventarioLibros
                .Include(i => i.IdInventarioNavigation)
                .Include(i => i.IdLibroNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inventarioLibro == null)
            {
                return NotFound();
            }

            return View(inventarioLibro);
        }

        // GET: InventarioLibroes/Create
        public IActionResult Create()
        {
            ViewData["IdLibro"] = new SelectList(_context.Libros, "Id", "Titulo");
            return View();
        }

        // POST: InventarioLibroes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InventarioLibro inventarioLibro, int cantidadReal, string observaciones)
        {
            if (ModelState.IsValid)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    // Validar las observaciones
                    observaciones = string.IsNullOrWhiteSpace(observaciones) ? "Sin observaciones" : observaciones;

                    // Crear un nuevo registro en la tabla Inventario
                    var nuevoInventario = new Inventario
                    {
                        CantidadReal = cantidadReal > 0 ? cantidadReal : 0, // Validar cantidad real positiva
                        CantidadSistema = cantidadReal > 0 ? cantidadReal : 0, // Inicializar CantidadSistema igual a CantidadReal
                        Observaciones = observaciones
                    };

                    _context.Inventarios.Add(nuevoInventario);
                    await _context.SaveChangesAsync();

                    // Asociar el nuevo inventario al InventarioLibro
                    inventarioLibro.IdInventario = nuevoInventario.Id;

                    // Validar los valores del modelo InventarioLibro
                    inventarioLibro.FechaApertura = inventarioLibro.FechaApertura == DateTime.MinValue ? DateTime.Now : inventarioLibro.FechaApertura;
                    inventarioLibro.FechaCierre = inventarioLibro.FechaCierre == DateTime.MinValue ? DateTime.Now.AddMonths(1) : inventarioLibro.FechaCierre;
                    inventarioLibro.InventarioTipo = string.IsNullOrWhiteSpace(inventarioLibro.InventarioTipo) ? "General" : inventarioLibro.InventarioTipo;

                    // Agregar el nuevo registro en la tabla InventarioLibro
                    _context.InventarioLibros.Add(inventarioLibro);
                    await _context.SaveChangesAsync();

                    // Confirmar la transacción
                    await transaction.CommitAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    ModelState.AddModelError("", $"Error al guardar: {ex.Message}");
                }
            }

            // Si falla, recargar las listas desplegables para evitar errores en la vista
            ViewData["IdLibro"] = new SelectList(_context.Libros, "Id", "Titulo", inventarioLibro.IdLibro);
            return View(inventarioLibro);
        }


        // POST: InventarioLibroes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdInventario,IdLibro,FechaApertura,FechaCierre,InventarioTipo")] InventarioLibro inventarioLibro)
        {
            if (id != inventarioLibro.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventarioLibro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventarioLibroExists(inventarioLibro.Id))
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
            ViewData["IdInventario"] = new SelectList(_context.Inventarios, "Id", "Id", inventarioLibro.IdInventario);
            ViewData["IdLibro"] = new SelectList(_context.Libros, "Id", "Id", inventarioLibro.IdLibro);
            return View(inventarioLibro);
        }

        // GET: InventarioLibroes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.InventarioLibros == null)
            {
                return NotFound();
            }

            var inventarioLibro = await _context.InventarioLibros
                .Include(i => i.IdInventarioNavigation)
                .Include(i => i.IdLibroNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inventarioLibro == null)
            {
                return NotFound();
            }

            return View(inventarioLibro);
        }

        // POST: InventarioLibroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.InventarioLibros == null)
            {
                return Problem("Entity set 'LobotecaContext.InventarioLibros'  is null.");
            }
            var inventarioLibro = await _context.InventarioLibros.FindAsync(id);
            if (inventarioLibro != null)
            {
                _context.InventarioLibros.Remove(inventarioLibro);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventarioLibroExists(int id)
        {
          return (_context.InventarioLibros?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
