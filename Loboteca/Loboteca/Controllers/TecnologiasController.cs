using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Loboteca.Models;

namespace Loboteca.Controllers
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
            // Filtramos los 6 libros más recientes que pertenecen a la carrera de Biotecnología (suponiendo IdCarrera = 5)
            var generosLibros = new List<string> { "Programación", "Tronco Comun" };
            var librosRecientes = _context.ELibros
                .Where(l => generosLibros.Contains(l.Genero))
                .OrderByDescending(l => l.Id)
                .Take(6)
                .ToList();

            // Filtramos las 6 revistas más recientes que pertenecen a la carrera de Biotecnología
            var generosRevistas = new List<string> { "Programación", "Tronco Comun" };
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
