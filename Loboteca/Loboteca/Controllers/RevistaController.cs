using System;
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
            ViewData["IdEditorial"] = new SelectList(_context.Editorials, "Id", "Nombre");
            ViewData["IdAutor"] = new SelectList(_context.Autors, "Id", "Nombre"); // Aseguramos que los autores estén disponibles
            return View();
        }

        // POST: Revista/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Revistum revista, IFormFile Imagen, IFormFile Archivo, int IdAutor)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Procesar imagen
                if (Imagen != null && Imagen.Length > 0)
                {
                    string imagePath = Path.Combine("wwwroot/images", Guid.NewGuid() + Path.GetExtension(Imagen.FileName));
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await Imagen.CopyToAsync(stream);
                    }
                    revista.RutaDeImagen = imagePath.Replace("wwwroot", "").Replace("\\", "/");
                }

                // Procesar archivo PDF
                if (Archivo != null && Archivo.Length > 0)
                {
                    string pdfPath = Path.Combine("wwwroot/pdf", Guid.NewGuid() + Path.GetExtension(Archivo.FileName));
                    using (var stream = new FileStream(pdfPath, FileMode.Create))
                    {
                        await Archivo.CopyToAsync(stream);
                    }
                    revista.Archivo = pdfPath.Replace("wwwroot", "").Replace("\\", "/");
                }

                // Guardar revista
                revista.FechaDeAlta = DateTime.Now;
                _context.Revista.Add(revista);
                await _context.SaveChangesAsync();

                // Crear relación en AutorRevista
                var autorRevista = new AutorRevistum
                {
                    IdAutor = IdAutor,
                    IdRevista = revista.Id
                };
                _context.AutorRevista.Add(autorRevista);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError("", $"Error al guardar los datos: {ex.Message}");
                ViewData["IdEditorial"] = new SelectList(_context.Editorials, "Id", "Nombre", revista.IdEditorial);
                ViewData["IdAutor"] = new SelectList(_context.Autors, "Id", "Nombre", IdAutor);
                return View(revista);
            }
        }

        // POST: Revista/Edit/5
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
