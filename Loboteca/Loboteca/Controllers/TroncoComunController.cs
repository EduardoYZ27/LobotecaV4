using Microsoft.AspNetCore.Mvc;
using Loboteca.Models;
using System.Linq;
using Loboteca.Models;

namespace Loboteca1.Controllers
{
    public class TroncoComunController : Controller
    {
        private readonly LobotecaContext _context;

        public TroncoComunController(LobotecaContext context)
        {
            _context = context;
        }

        // Mantén el nombre de la acción como TroncoComun para que coincida con el nombre de la vista
        public IActionResult TroncoComun()
        {
            // Filtrar los libros que pertenecen al género "Matemáticas"
            var librosMatematicas = _context.ELibros
                .Where(l => l.Genero.ToLower() == "matematicas")
                .OrderByDescending(l => l.FechaDePublicacion)
                .Take(6) // Solo tomamos los 6 más recientes
                .ToList();

            // Filtrar los libros que pertenecen al género "Física"
            var librosFisica = _context.ELibros
                .Where(l => l.Genero.ToLower() == "fisica")
                .OrderByDescending(l => l.FechaDePublicacion)
                .Take(6)
                .ToList();

            // Filtrar los libros que pertenecen al género "Química"
            var librosQuimica = _context.ELibros
                .Where(l => l.Genero.ToLower() == "quimica")
                .OrderByDescending(l => l.FechaDePublicacion)
                .Take(6)
                .ToList();

            // Filtrar los libros que pertenecen al género "Economía y Administración"
            var librosEconomia = _context.ELibros
                .Where(l => l.Genero.ToLower() == "economía y administración")
                .OrderByDescending(l => l.FechaDePublicacion)
                .Take(6)
                .ToList();

            // Filtrar los libros que pertenecen al género "Comunicación y Habilidades Blandas"
            var librosComunicacion = _context.ELibros
                .Where(l => l.Genero.ToLower() == "comunicación y habilidades blandas")
                .OrderByDescending(l => l.FechaDePublicacion)
                .Take(6)
                .ToList();

            // Pasamos los libros a la vista utilizando ViewBag
            ViewBag.LibrosMatematicas = librosMatematicas;
            ViewBag.LibrosFisica = librosFisica;
            ViewBag.LibrosQuimica = librosQuimica;
            ViewBag.LibrosEconomia = librosEconomia;
            ViewBag.LibrosComunicacion = librosComunicacion;

            return View(); // Aquí especifica que busque la vista "TroncoComun"
        }
    }
}
