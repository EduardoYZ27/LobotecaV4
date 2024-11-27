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
            // Filtramos los 6 libros más recientes que pertenecen a la carrera de Biotecnología (suponiendo IdCarrera = 5)
            var generosLibros = new List<string> { "Aduanas", "Tronco Comun" };
            var librosRecientes = _context.ELibros
                .Where(l => generosLibros.Contains(l.Genero))
                .OrderByDescending(l => l.Id)
                .Take(6)
                .ToList();

            // Filtramos las 6 revistas más recientes que pertenecen a la carrera de Biotecnología
            var generosRevistas = new List<string> { "Aduanas", "Tronco Comun" };
            var revistasRecientes = _context.Revista
                .Where(r => generosRevistas.Contains(r.Genero))
                .OrderByDescending(r => r.Id)
                .Take(6)
                .ToList();

            // Pasamos los datos a la vista
            ViewBag.LibrosRecientes = librosRecientes;
            ViewBag.RevistasRecientes = revistasRecientes;

            return View();
        }
    }
}
