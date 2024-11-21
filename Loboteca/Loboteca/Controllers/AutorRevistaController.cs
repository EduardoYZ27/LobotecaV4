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
    public class AutorRevistaController : Controller
    {
        private readonly LobotecaContext _context;

        public AutorRevistaController(LobotecaContext context)
        {
            _context = context;
        }

        // GET: AutorRevista
        public async Task<IActionResult> Index()
        {
            var lobotecaContext = _context.AutorRevista.Include(a => a.IdAutorNavigation).Include(a => a.IdRevistaNavigation);
            return View(await lobotecaContext.ToListAsync());
        }

        // GET: AutorRevista/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AutorRevista == null)
            {
                return NotFound();
            }

            var autorRevistum = await _context.AutorRevista
                .Include(a => a.IdAutorNavigation)
                .Include(a => a.IdRevistaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (autorRevistum == null)
            {
                return NotFound();
            }

            return View(autorRevistum);
        }

        // GET: AutorRevista/Create
        public IActionResult Create()
        {
            ViewData["IdAutor"] = new SelectList(_context.Autors, "Id", "Id");
            ViewData["IdRevista"] = new SelectList(_context.Revista, "Id", "Id");
            return View();
        }

        // POST: AutorRevista/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdAutor,IdRevista")] AutorRevistum autorRevistum)
        {
            if (ModelState.IsValid)
            {
                _context.Add(autorRevistum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAutor"] = new SelectList(_context.Autors, "Id", "Id", autorRevistum.IdAutor);
            ViewData["IdRevista"] = new SelectList(_context.Revista, "Id", "Id", autorRevistum.IdRevista);
            return View(autorRevistum);
        }

        // GET: AutorRevista/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AutorRevista == null)
            {
                return NotFound();
            }

            var autorRevistum = await _context.AutorRevista.FindAsync(id);
            if (autorRevistum == null)
            {
                return NotFound();
            }
            ViewData["IdAutor"] = new SelectList(_context.Autors, "Id", "Id", autorRevistum.IdAutor);
            ViewData["IdRevista"] = new SelectList(_context.Revista, "Id", "Id", autorRevistum.IdRevista);
            return View(autorRevistum);
        }

        // POST: AutorRevista/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdAutor,IdRevista")] AutorRevistum autorRevistum)
        {
            if (id != autorRevistum.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(autorRevistum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AutorRevistumExists(autorRevistum.Id))
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
            ViewData["IdAutor"] = new SelectList(_context.Autors, "Id", "Id", autorRevistum.IdAutor);
            ViewData["IdRevista"] = new SelectList(_context.Revista, "Id", "Id", autorRevistum.IdRevista);
            return View(autorRevistum);
        }

        // GET: AutorRevista/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AutorRevista == null)
            {
                return NotFound();
            }

            var autorRevistum = await _context.AutorRevista
                .Include(a => a.IdAutorNavigation)
                .Include(a => a.IdRevistaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (autorRevistum == null)
            {
                return NotFound();
            }

            return View(autorRevistum);
        }

        // POST: AutorRevista/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AutorRevista == null)
            {
                return Problem("Entity set 'LobotecaContext.AutorRevista'  is null.");
            }
            var autorRevistum = await _context.AutorRevista.FindAsync(id);
            if (autorRevistum != null)
            {
                _context.AutorRevista.Remove(autorRevistum);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AutorRevistumExists(int id)
        {
          return (_context.AutorRevista?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
