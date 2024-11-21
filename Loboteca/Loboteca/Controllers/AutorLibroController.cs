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
    public class AutorLibroController : Controller
    {
        private readonly LobotecaContext _context;

        public AutorLibroController(LobotecaContext context)
        {
            _context = context;
        }

        // GET: AutorLibro
        public async Task<IActionResult> Index()
        {
            var lobotecaContext = _context.AutorLibros.Include(a => a.IdAutorNavigation).Include(a => a.IdLibroNavigation);
            return View(await lobotecaContext.ToListAsync());
        }

        // GET: AutorLibro/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AutorLibros == null)
            {
                return NotFound();
            }

            var autorLibro = await _context.AutorLibros
                .Include(a => a.IdAutorNavigation)
                .Include(a => a.IdLibroNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (autorLibro == null)
            {
                return NotFound();
            }

            return View(autorLibro);
        }

        // GET: AutorLibro/Create
        public IActionResult Create()
        {
            ViewData["IdAutor"] = new SelectList(_context.Autors, "Id", "Id");
            ViewData["IdLibro"] = new SelectList(_context.Libros, "Id", "Id");
            return View();
        }

        // POST: AutorLibro/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdAutor,IdLibro")] AutorLibro autorLibro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(autorLibro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAutor"] = new SelectList(_context.Autors, "Id", "Id", autorLibro.IdAutor);
            ViewData["IdLibro"] = new SelectList(_context.Libros, "Id", "Id", autorLibro.IdLibro);
            return View(autorLibro);
        }

        // GET: AutorLibro/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AutorLibros == null)
            {
                return NotFound();
            }

            var autorLibro = await _context.AutorLibros.FindAsync(id);
            if (autorLibro == null)
            {
                return NotFound();
            }
            ViewData["IdAutor"] = new SelectList(_context.Autors, "Id", "Id", autorLibro.IdAutor);
            ViewData["IdLibro"] = new SelectList(_context.Libros, "Id", "Id", autorLibro.IdLibro);
            return View(autorLibro);
        }

        // POST: AutorLibro/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdAutor,IdLibro")] AutorLibro autorLibro)
        {
            if (id != autorLibro.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(autorLibro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AutorLibroExists(autorLibro.Id))
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
            ViewData["IdAutor"] = new SelectList(_context.Autors, "Id", "Id", autorLibro.IdAutor);
            ViewData["IdLibro"] = new SelectList(_context.Libros, "Id", "Id", autorLibro.IdLibro);
            return View(autorLibro);
        }

        // GET: AutorLibro/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AutorLibros == null)
            {
                return NotFound();
            }

            var autorLibro = await _context.AutorLibros
                .Include(a => a.IdAutorNavigation)
                .Include(a => a.IdLibroNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (autorLibro == null)
            {
                return NotFound();
            }

            return View(autorLibro);
        }

        // POST: AutorLibro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AutorLibros == null)
            {
                return Problem("Entity set 'LobotecaContext.AutorLibros'  is null.");
            }
            var autorLibro = await _context.AutorLibros.FindAsync(id);
            if (autorLibro != null)
            {
                _context.AutorLibros.Remove(autorLibro);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AutorLibroExists(int id)
        {
          return (_context.AutorLibros?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
