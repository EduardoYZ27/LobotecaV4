using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Necesario para Include
using Loboteca.Models;

namespace Loboteca1.Controllers
{
    public class Info_UserController : Controller
    {
        private readonly LobotecaContext _context;

        public Info_UserController(LobotecaContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Info_User()
        {
            // Recupera la matrícula de la sesión
            var matricula = HttpContext.Session.GetString("UsuarioMatricula");

            if (string.IsNullOrEmpty(matricula))
            {
                // Redirige al login si no hay sesión activa
                return RedirectToAction("Login", "Login");
            }

            // Busca el usuario en la base de datos usando la matrícula
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Matricula == matricula);

            if (usuario == null)
            {
                TempData["ErrorMessage"] = "No se pudo encontrar al usuario.";
                return RedirectToAction("Login", "Login");
            }

            // Pasa el rol y la carrera a la vista
            ViewBag.UsuarioRol = usuario.Rol;
            ViewBag.UsuarioIdCarrera = usuario.IdCarrera;

            return View(usuario);
        }


    }
}
