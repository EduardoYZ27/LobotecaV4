using Microsoft.AspNetCore.Mvc;
using Loboteca.Models;
using System.Linq;
using System.Collections.Generic;
using Loboteca.Models;

namespace Loboteca1.Controllers
{
    public class MecanicaController : Controller
    {
        private readonly LobotecaContext _context;

        public MecanicaController(LobotecaContext context)
        {
            _context = context;
        }

        public IActionResult Mecanica()
        {
            // Suponiendo que el ID de la carrera de Mecánica es 5
            var idCarreraMecanica = 3;

            // Obtener los 6 libros más recientes para la carrera de Mecánica
            var librosRecientes = _context.ELibros
                .Where(l => l.Id == idCarreraMecanica)
                .OrderByDescending(l => l.Id)
                .Take(6)
                .ToList();

            // Obtener las 6 revistas más recientes para la carrera de Mecánica
            var revistasRecientes = _context.Revista
                .Where(r => r.Id == idCarreraMecanica)
                .OrderByDescending(r => r.Id)
                .Take(6)
                .ToList();

            // Pasar los libros y revistas a la vista usando ViewBag
            ViewBag.LibrosRecientes = librosRecientes;
            ViewBag.RevistasRecientes = revistasRecientes;

            return View();
        }
    }
}
