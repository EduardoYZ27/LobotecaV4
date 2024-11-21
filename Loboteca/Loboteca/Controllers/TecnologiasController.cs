using Microsoft.AspNetCore.Mvc;
using Loboteca.Models;
using System.Linq;
using Loboteca.Models;

namespace Loboteca1.Controllers
{
    public class TecnologiasController : Controller
    {
        private readonly LobotecaContext _context;

        public TecnologiasController(LobotecaContext context)
        {
            _context = context;
        }

        public IActionResult Tecnologias()
        {
            // Filtramos los 6 libros más recientes que pertenecen a la carrera de Tecnologías de la Información
            var librosRecientes = _context.ELibros
                .Where(l => l.Id == 5) // Suponiendo que 2 es el ID de Tecnologías de la Información
                .OrderByDescending(l => l.Id)
                .Take(6)
                .ToList();

            // Filtramos las 6 revistas más recientes que pertenecen a la carrera de Tecnologías de la Información
            var revistasRecientes = _context.Revista
                .Where(r => r.Id == 5) // Suponiendo que 2 es el ID de Tecnologías de la Información
                .OrderByDescending(r => r.Id)
                .Take(6)
                .ToList();

            // Pasamos ambos conjuntos de datos a la vista
            ViewBag.LibrosRecientes = librosRecientes;
            ViewBag.RevistasRecientes = revistasRecientes;

            return View();
        }
    }
}
