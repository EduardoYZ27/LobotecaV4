using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Loboteca.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Loboteca.Controllers
{
    public class RegistroAlumnoController : Controller
    {
        private readonly LobotecaContext _context;

        public RegistroAlumnoController(LobotecaContext context)
        {
            _context = context;
        }

        // GET: RegistroAlumno/Create
        public IActionResult RegistroAlumno()
        {
            // Obtiene la lista de carreras activas para llenar el dropdown en la vista
            ViewBag.Carreras = _context.Carreras
                .Where(c => c.Estado == "Activo") // Solo carreras activas
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Nombre
                })
                .ToList();

            return View();
        }

        // POST: RegistroAlumno/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistroAlumno([Bind("Nombre,ApellidoPaterno,ApellidoMaterno,Matricula,Estado,Contra,Rol,IdCarrera")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Agrega el nuevo usuario al contexto
                    _context.Add(usuario);
                    await _context.SaveChangesAsync();

                    // Redirige a la misma página con un mensaje de éxito
                    TempData["SuccessMessage"] = "Alumno registrado con éxito.";
                    return RedirectToAction(nameof(RegistroAlumno));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al registrar al alumno: " + ex.Message);
                }
            }

            // Si hay un error, recarga las carreras y vuelve a mostrar el formulario
            ViewBag.Carreras = _context.Carreras
                .Where(c => c.Estado == "Activo")
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Nombre
                })
                .ToList();

            return View(usuario);
        }
    }
}
