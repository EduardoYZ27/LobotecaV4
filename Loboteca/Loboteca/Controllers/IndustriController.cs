using Microsoft.AspNetCore.Mvc;
using Loboteca.Models;
using System.Linq;
using Loboteca.Models;

namespace Loboteca1.Controllers
{
    public class IndustriController : Controller
    {
        private readonly LobotecaContext _context;

        public IndustriController(LobotecaContext context)
        {
            _context = context;
        }

        public IActionResult Industri()
        {
            // Filtramos los 6 libros más recientes que pertenecen a la carrera de Industrial
            var librosRecientes = _context.ELibros
                .Where(l => l.Id == 1) // Suponiendo que 1 es el ID de la carrera Industrial
                .OrderByDescending(l => l.Id)
                .Take(6)
                .ToList();

            // Filtramos las 6 revistas más recientes que pertenecen a la carrera de Industrial
            var revistasRecientes = _context.Revista
                .Where(r => r.Id == 1) // Suponiendo que 1 es el ID de la carrera Industrial
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
