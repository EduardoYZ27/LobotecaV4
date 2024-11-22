using Microsoft.AspNetCore.Mvc;

namespace Loboteca1.Controllers
{
    public class InicioController : Controller
    {
        [HttpGet]
        public IActionResult Inicio()
        {
            return View("/Views/Inicio.cshtml"); // Ruta completa
        }
    }
}
