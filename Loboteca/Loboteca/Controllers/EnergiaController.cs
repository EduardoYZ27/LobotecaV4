using Microsoft.AspNetCore.Mvc;
using Loboteca.Models;
using System.Linq;
using System.Collections.Generic;
using Loboteca.Models;

namespace Loboteca1.Controllers
{
    public class EnergiaController : Controller
    {
        private readonly LobotecaContext _context;

        public EnergiaController(LobotecaContext context)
        {
            _context = context;
        }

        public IActionResult Energia()
        {
            // Suponiendo que el ID de la carrera de Energía es 6
            var idCarreraEnergia = 6;

            // Obtener los 6 libros más recientes para la carrera de Energía
            var librosRecientes = _context.ELibros
                .Where(l => l.Id == idCarreraEnergia)
                .OrderByDescending(l => l.Id)
                .Take(6)
                .ToList();

            // Obtener las 6 revistas más recientes para la carrera de Energía
            var revistasRecientes = _context.Revista
                .Where(r => r.Id == idCarreraEnergia)
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
