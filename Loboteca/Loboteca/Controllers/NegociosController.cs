using Microsoft.AspNetCore.Mvc;
using Loboteca.Models;
using System.Linq;
using Loboteca.Models;

namespace Loboteca1.Controllers
{
    public class NegociosController : Controller
    {
        private readonly LobotecaContext _context;

        public NegociosController(LobotecaContext context)
        {
            _context = context;
        }

        public IActionResult Negocios()
        {
            // Suponiendo que el ID de la carrera de Negocios Internacionales es 6
            var idCarreraNegocios = 8;

            // Obtener los 6 libros más recientes para la carrera de Negocios Internacionales
            var librosRecientes = _context.ELibros
                .Where(l => l.Id == idCarreraNegocios)
                .OrderByDescending(l => l.Id)
                .Take(6)
                .ToList();

            // Obtener las 6 revistas más recientes para la carrera de Negocios Internacionales
            var revistasRecientes = _context.Revista
                .Where(r => r.Id == idCarreraNegocios)
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
