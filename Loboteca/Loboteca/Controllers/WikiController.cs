using Microsoft.AspNetCore.Mvc;
using Loboteca.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Loboteca.Controllers
{
    public class WikiController : Controller
    {
        private readonly MediaWikiService _mediaWikiService;

        public WikiController(MediaWikiService mediaWikiService)
        {
            _mediaWikiService = mediaWikiService;
        }

        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                ViewBag.Message = "Por favor, ingrese un término de búsqueda.";
                return View();
            }

            var results = await _mediaWikiService.SearchArticlesAsync(query);
            if (results.Count == 0)
            {
                ViewBag.Message = "No se encontraron resultados.";
            }

            return View(results);
        }
    }
}
