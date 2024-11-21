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
    public class ELibroController : Controller
    {
        private readonly LobotecaContext _context;

        public ELibroController(LobotecaContext context)
        {
            _context = context;
        }

        // GET: ELibro
        public async Task<IActionResult> Index()
        {
            var lobotecaContext = _context.ELibros.Include(e => e.IdEditorialNavigation);
            return View(await lobotecaContext.ToListAsync());
        }

        // GET: ELibro/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ELibros == null)
            {
                return NotFound();
            }

            var eLibro = await _context.ELibros
                .Include(e => e.IdEditorialNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eLibro == null)
            {
                return NotFound();
            }

            return View(eLibro);
        }

        // GET: ELibro/Create
        public IActionResult Create()
        {
            ViewData["IdEditorial"] = new SelectList(_context.Editorials, "Id", "Id");
            return View();
        }

        // POST: ELibro/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Isbn,FechaDePublicacion,Genero,Estado,RutaDeImagen,Archivo,FechaDeAlta,IdEditorial")] ELibro eLibro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eLibro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEditorial"] = new SelectList(_context.Editorials, "Id", "Id", eLibro.IdEditorial);
            return View(eLibro);
        }

        // GET: ELibro/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ELibros == null)
            {
                return NotFound();
            }

            var eLibro = await _context.ELibros.FindAsync(id);
            if (eLibro == null)
            {
                return NotFound();
            }
            ViewData["IdEditorial"] = new SelectList(_context.Editorials, "Id", "Id", eLibro.IdEditorial);
            return View(eLibro);
        }

        // POST: ELibro/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Isbn,FechaDePublicacion,Genero,Estado,RutaDeImagen,Archivo,FechaDeAlta,IdEditorial")] ELibro eLibro)
        {
            if (id != eLibro.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eLibro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ELibroExists(eLibro.Id))
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
            ViewData["IdEditorial"] = new SelectList(_context.Editorials, "Id", "Id", eLibro.IdEditorial);
            return View(eLibro);
        }

        // GET: ELibro/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ELibros == null)
            {
                return NotFound();
            }

            var eLibro = await _context.ELibros
                .Include(e => e.IdEditorialNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eLibro == null)
            {
                return NotFound();
            }

            return View(eLibro);
        }

        // POST: ELibro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ELibros == null)
            {
                return Problem("Entity set 'LobotecaContext.ELibros'  is null.");
            }
            var eLibro = await _context.ELibros.FindAsync(id);
            if (eLibro != null)
            {
                _context.ELibros.Remove(eLibro);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ELibroExists(int id)
        {
          return (_context.ELibros?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
