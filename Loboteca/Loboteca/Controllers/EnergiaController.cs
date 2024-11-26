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
            // Filtramos los 6 libros más recientes que pertenecen a la carrera de Biotecnología (suponiendo IdCarrera = 5)
            var generosLibros = new List<string> { "Energia", "Programación", "Tronco Comun" };
            var librosRecientes = _context.ELibros
                .Where(l => generosLibros.Contains(l.Genero))
                .OrderByDescending(l => l.Id)
                .Take(6)
                .ToList();

            // Filtramos las 6 revistas más recientes que pertenecen a la carrera de Biotecnología
            var generosRevistas = new List<string> { "Energia", "Programacion", "Tronco Comun" };
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
