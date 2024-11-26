using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Loboteca.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            ViewData["IdEditorial"] = new SelectList(_context.Editorials, "Id", "Nombre");
            ViewData["IdAutor"] = new SelectList(_context.Autors, "Id", "Nombre");
            return View();
        }

        // POST: ELibro/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ELibro eLibro, IFormFile Imagen, IFormFile Archivo, int IdAutor)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Validar y guardar la imagen
                if (Imagen != null && Imagen.Length > 0)
                {
                    string imagePath = Path.Combine("wwwroot/images", Guid.NewGuid() + Path.GetExtension(Imagen.FileName));
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await Imagen.CopyToAsync(stream);
                    }
                    eLibro.RutaDeImagen = imagePath.Replace("wwwroot", "").Replace("\\", "/");
                }

                // Validar y guardar el archivo PDF
                if (Archivo != null && Archivo.Length > 0)
                {
                    string pdfPath = Path.Combine("wwwroot/pdf", Guid.NewGuid() + Path.GetExtension(Archivo.FileName));
                    using (var stream = new FileStream(pdfPath, FileMode.Create))
                    {
                        await Archivo.CopyToAsync(stream);
                    }
                    eLibro.Archivo = pdfPath.Replace("wwwroot", "").Replace("\\", "/");
                }

                // Guardar el libro electrónico
                eLibro.FechaDeAlta = DateTime.Now;
                _context.ELibros.Add(eLibro);
                await _context.SaveChangesAsync();

                // Crear la relación en la tabla intermedia AutorELibro
                var autorELibro = new AutorELibro
                {
                    IdAutor = IdAutor,
                    IdELibro = eLibro.Id
                };
                _context.AutorELibros.Add(autorELibro);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError("", $"Error al guardar el registro: {ex.Message}");
                ViewData["IdEditorial"] = new SelectList(_context.Editorials, "Id", "Nombre", eLibro.IdEditorial);
                ViewData["IdAutor"] = new SelectList(_context.Autors, "Id", "Nombre", IdAutor);
                return View(eLibro);
            }
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
            ViewData["IdEditorial"] = new SelectList(_context.Editorials, "Id", "Nombre", eLibro.IdEditorial);
            return View(eLibro);
        }

        // POST: ELibro/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ELibro eLibro, IFormFile Imagen, IFormFile Archivo)
        {
            if (id != eLibro.Id)
            {
                return NotFound();
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Obtener el registro existente
                var existingELibro = await _context.ELibros.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
                if (existingELibro == null)
                {
                    return NotFound();
                }

                // Validar y procesar la imagen
                if (Imagen != null && Imagen.Length > 0)
                {
                    string imagePath = Path.Combine("wwwroot/images", Guid.NewGuid() + Path.GetExtension(Imagen.FileName));
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await Imagen.CopyToAsync(stream);
                    }
                    eLibro.RutaDeImagen = imagePath.Replace("wwwroot", "").Replace("\\", "/");
                }
                else
                {
                    eLibro.RutaDeImagen = existingELibro.RutaDeImagen;
                }

                // Validar y procesar el archivo PDF
                if (Archivo != null && Archivo.Length > 0)
                {
                    string pdfPath = Path.Combine("wwwroot/pdf", Guid.NewGuid() + Path.GetExtension(Archivo.FileName));
                    using (var stream = new FileStream(pdfPath, FileMode.Create))
                    {
                        await Archivo.CopyToAsync(stream);
                    }
                    eLibro.Archivo = pdfPath.Replace("wwwroot", "").Replace("\\", "/");
                }
                else
                {
                    eLibro.Archivo = existingELibro.Archivo;
                }

                // Actualizar el registro
                _context.Update(eLibro);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError("", $"Error al actualizar el registro: {ex.Message}");
                ViewData["IdEditorial"] = new SelectList(_context.Editorials, "Id", "Nombre", eLibro.IdEditorial);
                return View(eLibro);
            }
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
                return Problem("Entity set 'LobotecaContext.ELibros' is null.");
            }

            var eLibro = await _context.ELibros.FindAsync(id);
            if (eLibro != null)
            {
                // Eliminar registros relacionados en la tabla intermedia AutorELibro
                var autorELibros = _context.AutorELibros.Where(ae => ae.IdELibro == id);
                _context.AutorELibros.RemoveRange(autorELibros);

                // Eliminar la imagen asociada al eLibro
                if (!string.IsNullOrEmpty(eLibro.RutaDeImagen) && !eLibro.RutaDeImagen.Contains("default.jpg"))
                {
                    var imagePath = Path.Combine("wwwroot", eLibro.RutaDeImagen.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                // Eliminar el archivo asociado al eLibro
                if (!string.IsNullOrEmpty(eLibro.Archivo))
                {
                    var pdfPath = Path.Combine("wwwroot", eLibro.Archivo.TrimStart('/'));
                    if (System.IO.File.Exists(pdfPath))
                    {
                        System.IO.File.Delete(pdfPath);
                    }
                }

                // Eliminar el registro del eLibro
                _context.ELibros.Remove(eLibro);
            }

            // Guardar los cambios
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ELibroExists(int id)
        {
            return _context.ELibros.Any(e => e.Id == id);
        }
    }
}
