using Microsoft.AspNetCore.Mvc;

namespace Loboteca1.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // Lógica de autenticación básica
            if (username == "admin" && password == "admin")
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ErrorMessage = "Usuario o contraseña incorrectos.";
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
    }
}
