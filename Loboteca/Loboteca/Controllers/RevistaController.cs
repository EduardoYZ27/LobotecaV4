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

            ViewData["IdEditorial"] = new SelectList(_context.Editorials, "Id", "Nombre", revistum.IdEditorial);
            return View(revistum);
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
        public async Task<IActionResult> Edit(int id, Revistum revista, IFormFile Imagen, IFormFile Archivo)
        {
            if (id != revista.Id)
            {
                return NotFound();
            }

            try
            {
                // Procesar la imagen si se seleccionó una nueva
                if (Imagen != null && Imagen.Length > 0)
                {
                    string imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    if (!Directory.Exists(imageFolder))
                    {
                        Directory.CreateDirectory(imageFolder);
                    }

                    string uniqueImageName = Guid.NewGuid().ToString() + Path.GetExtension(Imagen.FileName);
                    string imagePath = Path.Combine(imageFolder, uniqueImageName);

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await Imagen.CopyToAsync(stream);
                    }

                    // Eliminar la imagen anterior si existe y no es la predeterminada
                    if (!string.IsNullOrEmpty(revista.RutaDeImagen) && !revista.RutaDeImagen.Contains("default.jpg"))
                    {
                        string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", revista.RutaDeImagen.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    revista.RutaDeImagen = $"/images/{uniqueImageName}";
                }

                // Procesar el archivo PDF si se seleccionó uno nuevo
                if (Archivo != null && Archivo.Length > 0)
                {
                    string pdfFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/pdf");
                    if (!Directory.Exists(pdfFolder))
                    {
                        Directory.CreateDirectory(pdfFolder);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(Archivo.FileName);
                    string pdfPath = Path.Combine(pdfFolder, uniqueFileName);

                    using (var stream = new FileStream(pdfPath, FileMode.Create))
                    {
                        await Archivo.CopyToAsync(stream);
                    }

                    // Eliminar el archivo PDF anterior si existe
                    if (!string.IsNullOrEmpty(revista.Archivo))
                    {
                        string oldPdfPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", revista.Archivo.TrimStart('/'));
                        if (System.IO.File.Exists(oldPdfPath))
                        {
                            System.IO.File.Delete(oldPdfPath);
                        }
                    }

                    revista.Archivo = $"/pdf/{uniqueFileName}";
                }

                // Actualizar los datos de la revista
                _context.Update(revista);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
<<<<<<< HEAD
            catch (DbUpdateConcurrencyException)
            {
                if (!RevistumExists(revista.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al actualizar los datos: {ex.Message}");
            }

            // Volver a cargar las opciones de editoriales en caso de error
            ViewData["IdEditorial"] = new SelectList(_context.Editorials, "Id", "Nombre", revista.IdEditorial);
            return View(revista);
=======
            ViewData["IdEditorial"] = new SelectList(_context.Editorials, "Id", "Nombre", revistum.IdEditorial);
            return View(revistum);
>>>>>>> 221e2f61ba7d513e841793ee53431e6634bcfa7e
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
                return Problem("Entity set 'LobotecaContext.Revista' is null.");
            }

            // Obtener el registro de la revista
            var revistum = await _context.Revista.FindAsync(id);
            if (revistum != null)
            {
                // Eliminar los registros relacionados en la tabla intermedia AutorRevista
                var autorRevistas = _context.AutorRevista.Where(ar => ar.IdRevista == id);
                _context.AutorRevista.RemoveRange(autorRevistas);

                // Eliminar la revista
                _context.Revista.Remove(revistum);

                // Guardar cambios en la base de datos
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
        private bool RevistumExists(int id)
        {
            return _context.Revista.Any(e => e.Id == id);
        }
    }
}
