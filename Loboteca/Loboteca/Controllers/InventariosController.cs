using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Loboteca.Models;

namespace Loboteca.Controllers
{
    public class InventariosController : Controller
    {
        private readonly LobotecaContext _context;

        public InventariosController(LobotecaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Obtener todos los inventarios
            var inventarios = await _context.Inventarios
                .Include(i => i.InventarioLibros)
                .ThenInclude(il => il.IdLibroNavigation)
                .ToListAsync();

            // Calcular la cantidad en sistema dinámicamente
            foreach (var inventario in inventarios)
            {
                // Obtener el ID del libro asociado al inventario
                var libroId = inventario.InventarioLibros.FirstOrDefault()?.IdLibro;

                if (libroId != null)
                {
                    // Sumar ingresos para este libro
                    var totalIngresos = _context.Ingresos
                        .Where(i => i.IdLibro == libroId)
                        .Sum(i => i.Ejemplares);

                    // Restar préstamos activos para este libro
                    var totalPrestamos = _context.Prestamos
                        .Where(p => p.IdLibro == libroId && !_context.Devoluciones.Any(d => d.IdPrestamo == p.Id))
                        .Count();

                    // Calcular la cantidad en sistema
                    inventario.CantidadSistema = totalIngresos - totalPrestamos;
                }
                else
                {
                    inventario.CantidadSistema = 0; // Si no hay libros asociados, se considera 0
                }
            }

            return View(inventarios);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var inventario = await _context.Inventarios
                .Include(i => i.InventarioLibros)
                .ThenInclude(il => il.IdLibroNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (inventario == null)
                return NotFound();

            return View(inventario);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CantidadReal,Observaciones")] Inventario inventario)
        {
            if (ModelState.IsValid)
            {
                // Inicializa CantidadSistema como 0, ya que es un dato calculado.
                inventario.CantidadSistema = 0;

                _context.Add(inventario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(inventario);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var inventario = await _context.Inventarios.FindAsync(id);
            if (inventario == null)
                return NotFound();

            return View(inventario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CantidadReal,CantidadSistema,Observaciones")] Inventario inventario)
        {
            if (id != inventario.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventarioExists(inventario.Id))
                        return NotFound();

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(inventario);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var inventario = await _context.Inventarios.FindAsync(id);
            if (inventario == null)
                return NotFound();

            return View(inventario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inventario = await _context.Inventarios.FindAsync(id);
            if (inventario != null)
                _context.Inventarios.Remove(inventario);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventarioExists(int id)
        {
            return _context.Inventarios.Any(e => e.Id == id);
        }
    }
}
