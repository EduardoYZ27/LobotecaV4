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
    public class RevistaController : Controller
    {
        private readonly LobotecaContext _context;

        public RevistaController(LobotecaContext context)
        {
            _context = context;
        }

        // GET: Revista
        public async Task<IActionResult> Index()
        {
            var lobotecaContext = _context.Revista.Include(r => r.IdEditorialNavigation);
            return View(await lobotecaContext.ToListAsync());
        }

        // GET: Revista/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Revista == null)
            {
                return NotFound();
            }

            var revistum = await _context.Revista
                .Include(r => r.IdEditorialNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (revistum == null)
            {
                return NotFound();
            }

            return View(revistum);
        }

        // GET: Revista/Create
        public IActionResult Create()
        {
            ViewData["IdEditorial"] = new SelectList(_context.Editorials, "Id", "Id");
            return View();
        }

        // POST: Revista/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Issn,FechaDePublicacion,Genero,Estado,RutaDeImagen,Archivo,FechaDeAlta,IdEditorial")] Revistum revistum)
        {
            if (ModelState.IsValid)
            {
                _context.Add(revistum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEditorial"] = new SelectList(_context.Editorials, "Id", "Id", revistum.IdEditorial);
            return View(revistum);
        }

        // GET: Revista/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Revista == null)
            {
                return NotFound();
            }

            var revistum = await _context.Revista.FindAsync(id);
            if (revistum == null)
            {
                return NotFound();
            }
            ViewData["IdEditorial"] = new SelectList(_context.Editorials, "Id", "Id", revistum.IdEditorial);
            return View(revistum);
        }

        // POST: Revista/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Issn,FechaDePublicacion,Genero,Estado,RutaDeImagen,Archivo,FechaDeAlta,IdEditorial")] Revistum revistum)
        {
            if (id != revistum.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(revistum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RevistumExists(revistum.Id))
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
            ViewData["IdEditorial"] = new SelectList(_context.Editorials, "Id", "Id", revistum.IdEditorial);
            return View(revistum);
        }

        // GET: Revista/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Revista == null)
            {
                return NotFound();
            }

            var revistum = await _context.Revista
                .Include(r => r.IdEditorialNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (revistum == null)
            {
                return NotFound();
            }

            return View(revistum);
        }

        // POST: Revista/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Revista == null)
            {
                return Problem("Entity set 'LobotecaContext.Revista'  is null.");
            }
            var revistum = await _context.Revista.FindAsync(id);
            if (revistum != null)
            {
                _context.Revista.Remove(revistum);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RevistumExists(int id)
        {
          return (_context.Revista?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
