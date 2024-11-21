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
            var lobotecaContext = _context.InventarioLibros.Include(i => i.IdInventarioNavigation).Include(i => i.IdLibroNavigation);
            return View(await lobotecaContext.ToListAsync());
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
            ViewData["IdInventario"] = new SelectList(_context.Inventarios, "Id", "Id");
            ViewData["IdLibro"] = new SelectList(_context.Libros, "Id", "Id");
            return View();
        }

        // POST: InventarioLibroes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdInventario,IdLibro,FechaApertura,FechaCierre,InventarioTipo")] InventarioLibro inventarioLibro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inventarioLibro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdInventario"] = new SelectList(_context.Inventarios, "Id", "Id", inventarioLibro.IdInventario);
            ViewData["IdLibro"] = new SelectList(_context.Libros, "Id", "Id", inventarioLibro.IdLibro);
            return View(inventarioLibro);
        }

        // GET: InventarioLibroes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.InventarioLibros == null)
            {
                return NotFound();
            }

            var inventarioLibro = await _context.InventarioLibros.FindAsync(id);
            if (inventarioLibro == null)
            {
                return NotFound();
            }
            ViewData["IdInventario"] = new SelectList(_context.Inventarios, "Id", "Id", inventarioLibro.IdInventario);
            ViewData["IdLibro"] = new SelectList(_context.Libros, "Id", "Id", inventarioLibro.IdLibro);
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
