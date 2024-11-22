using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Loboteca.Models;

namespace Loboteca.Controllers
{
    public class LibroController : Controller
    {
        private readonly LobotecaContext _context;

        public LibroController(LobotecaContext context)
        {
            _context = context;
        }

        // GET: Libro
        public async Task<IActionResult> Index()
        {
            var lobotecaContext = _context.Libros.Include(l => l.IdEditorialNavigation);
            return View(await lobotecaContext.ToListAsync());
        }

        // GET: Libro/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Libros == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .Include(l => l.IdEditorialNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // GET: Libro/Create
        public IActionResult Create()
        {
            ViewData["IdEditorial"] = new SelectList(_context.Editorials, "Id", "Nombre");
            return View();
        }

        // POST: Libro/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Libro libro, IFormFile Imagen)
        {
            try
            {
                // Validar que el archivo de imagen fue enviado
                if (Imagen == null || Imagen.Length == 0)
                {
                    ModelState.AddModelError("RutaDeImagen", "Debe seleccionar una imagen válida.");
                    ViewData["IdEditorial"] = new SelectList(_context.Editorials, "Id", "Nombre", libro.IdEditorial);
                    return View(libro);
                }

                // Validar formatos permitidos
                var permittedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(Imagen.FileName).ToLowerInvariant();

                if (string.IsNullOrEmpty(extension) || !permittedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("RutaDeImagen", "Formato de imagen no permitido. Los formatos permitidos son: .jpg, .jpeg, .png, .gif.");
                    ViewData["IdEditorial"] = new SelectList(_context.Editorials, "Id", "Nombre", libro.IdEditorial);
                    return View(libro);
                }

                // Procesar la imagen
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(Imagen.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Imagen.CopyToAsync(stream);
                }

                libro.RutaDeImagen = $"/images/{uniqueFileName}";

                // Asignar valores por defecto si es necesario
                if (libro.FechaDeAlta == default(DateTime))
                {
                    libro.FechaDeAlta = DateTime.Now;
                }

                // Guardar en la base de datos
                _context.Add(libro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al guardar el registro: {ex.Message}");
                ViewData["IdEditorial"] = new SelectList(_context.Editorials, "Id", "Nombre", libro.IdEditorial);
                return View(libro);
            }
        }

        // GET: Libro/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Libros == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }

            ViewData["IdEditorial"] = new SelectList(_context.Editorials, "Id", "Nombre", libro.IdEditorial);
            return View(libro);
        }

        // POST: Libro/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Libro libro, IFormFile Imagen)
        {
            if (id != libro.Id)
            {
                return NotFound();
            }

            try
            {
                // Verificar si el modelo es válido
                if (!ModelState.IsValid)
                {
                    ViewData["IdEditorial"] = new SelectList(_context.Editorials, "Id", "Nombre", libro.IdEditorial);
                    return View(libro);
                }

                // Obtener el registro existente
                var existingLibro = await _context.Libros.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id);
                if (existingLibro == null)
                {
                    return NotFound();
                }

                // Procesar la imagen
                if (Imagen != null && Imagen.Length > 0)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(Imagen.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Imagen.CopyToAsync(stream);
                    }

                    libro.RutaDeImagen = $"/images/{uniqueFileName}";

                    if (!string.IsNullOrEmpty(existingLibro.RutaDeImagen) && !existingLibro.RutaDeImagen.Contains("default.jpg"))
                    {
                        var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingLibro.RutaDeImagen.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                }
                else
                {
                    libro.RutaDeImagen = existingLibro.RutaDeImagen;
                }

                // Actualizar el registro
                _context.Update(libro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al actualizar el registro: {ex.Message}");
                ViewData["IdEditorial"] = new SelectList(_context.Editorials, "Id", "Nombre", libro.IdEditorial);
                return View(libro);
            }
        }

        // GET: Libro/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Libros == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .Include(l => l.IdEditorialNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // POST: Libro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Libros == null)
            {
                return Problem("Entity set 'LobotecaContext.Libros' is null.");
            }

            var libro = await _context.Libros.FindAsync(id);
            if (libro != null)
            {
                // Eliminar la imagen asociada al libro
                if (!string.IsNullOrEmpty(libro.RutaDeImagen) && !libro.RutaDeImagen.Contains("default.jpg"))
                {
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", libro.RutaDeImagen.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.Libros.Remove(libro);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
