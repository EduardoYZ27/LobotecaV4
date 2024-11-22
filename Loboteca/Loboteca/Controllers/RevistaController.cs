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
        public async Task<IActionResult> Create(Revistum revista, IFormFile Imagen, IFormFile Archivo, int idAutor)
        {
            try
            {
                // Validar la imagen
                if (Imagen != null && Imagen.Length > 0)
                {
                    var permittedImageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var imageExtension = Path.GetExtension(Imagen.FileName).ToLowerInvariant();

                    if (string.IsNullOrEmpty(imageExtension) || !permittedImageExtensions.Contains(imageExtension))
                    {
                        ModelState.AddModelError("RutaDeImagen", "Formato de imagen no permitido. Solo se permiten .jpg, .jpeg, .png y .gif.");
                        ViewData["IdEditorial"] = new SelectList(_context.Editorials, "Id", "Nombre", revista.IdEditorial);
                        return View(revista);
                    }

                    string imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                    if (!Directory.Exists(imageFolder))
                        Directory.CreateDirectory(imageFolder);

                    string uniqueImageName = Guid.NewGuid().ToString() + Path.GetExtension(Imagen.FileName);
                    string imagePath = Path.Combine(imageFolder, uniqueImageName);

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                        await Imagen.CopyToAsync(stream);

                    revista.RutaDeImagen = $"/images/{uniqueImageName}";
                }
                else
                {
                    ModelState.AddModelError("RutaDeImagen", "La imagen es obligatoria.");
                    ViewData["IdEditorial"] = new SelectList(_context.Editorials, "Id", "Nombre", revista.IdEditorial);
                    return View(revista);
                }

                // Validar el archivo PDF
                if (Archivo != null && Archivo.Length > 0)
                {
                    var permittedFileExtensions = new[] { ".pdf" };
                    var fileExtension = Path.GetExtension(Archivo.FileName).ToLowerInvariant();

                    if (string.IsNullOrEmpty(fileExtension) || !permittedFileExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("Archivo", "Formato de archivo no permitido. Solo se permite .pdf.");
                        ViewData["IdEditorial"] = new SelectList(_context.Editorials, "Id", "Nombre", revista.IdEditorial);
                        return View(revista);
                    }

                    string pdfFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pdf");
                    if (!Directory.Exists(pdfFolder))
                        Directory.CreateDirectory(pdfFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(Archivo.FileName);
                    string pdfPath = Path.Combine(pdfFolder, uniqueFileName);

                    using (var stream = new FileStream(pdfPath, FileMode.Create))
                        await Archivo.CopyToAsync(stream);

                    revista.Archivo = $"/pdf/{uniqueFileName}";
                }
                else
                {
                    ModelState.AddModelError("Archivo", "El archivo PDF es obligatorio.");
                    ViewData["IdEditorial"] = new SelectList(_context.Editorials, "Id", "Nombre", revista.IdEditorial);
                    return View(revista);
                }

                // Asignar valores por defecto si es necesario
                if (revista.FechaDeAlta == default(DateTime))
                    revista.FechaDeAlta = DateTime.Now;

                // Guardar la revista
                _context.Add(revista);
                await _context.SaveChangesAsync();

                // Insertar en la tabla intermedia autor_revista después de guardar la revista
                if (idAutor != 0)
                {
                    var autorRevista = new AutorRevistum
                    {
                        IdAutor = idAutor,
                        IdRevista = revista.Id // Relacionar el autor con la revista recién creada
                    };

                    _context.Add(autorRevista);
                    await _context.SaveChangesAsync(); // Guardar la relación en la tabla autor_revista
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al guardar el registro: {ex.Message}");
                ViewData["IdEditorial"] = new SelectList(_context.Editorials, "Id", "Nombre", revista.IdEditorial);
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
