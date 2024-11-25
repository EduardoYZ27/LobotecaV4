using Microsoft.AspNetCore.Mvc;
using Loboteca.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Loboteca.Controllers
{
    public class SuggestionController : Controller
    {
        private readonly GoogleBooksService _googleBooksService;
        private static List<BookSuggestion> _cachedSuggestions = new();
        private static DateTime _lastUpdate = DateTime.MinValue;

        public SuggestionController(GoogleBooksService googleBooksService)
        {
            _googleBooksService = googleBooksService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Verificar si han pasado 24 horas desde la última actualización
            if ((DateTime.Now - _lastUpdate).TotalHours >= 24 || _cachedSuggestions.Count == 0)
            {
                var spanishBooks = await _googleBooksService.GetBookSuggestionsAsync("libros", "es");
                var englishBooks = await _googleBooksService.GetBookSuggestionsAsync("books", "en");

                _cachedSuggestions = new List<BookSuggestion>();
                _cachedSuggestions.AddRange(spanishBooks);
                _cachedSuggestions.AddRange(englishBooks);

                _lastUpdate = DateTime.Now;
            }

            return View(_cachedSuggestions);
        }
    }
}
