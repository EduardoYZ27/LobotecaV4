using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; // Importa para manejar sesiones
using Loboteca.Models;
using System.Linq;

namespace Loboteca1.Controllers
{
    public class LoginController : Controller
    {
        private readonly LobotecaContext _context;

        public LoginController(LobotecaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string matricula, string Contra)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Matricula == matricula);

            if (usuario == null)
            {
                TempData["ErrorMessage"] = "El usuario no está registrado.";
            }
            else if (usuario.Estado != "Activo")
            {
                TempData["ErrorMessage"] = "El usuario está inactivo.";
            }
            else if (usuario.Contra != Contra)
            {
                TempData["ErrorMessage"] = "Contraseña incorrecta.";
            }
            else
            {
                // Almacenar datos relevantes en la sesión
                HttpContext.Session.SetString("UsuarioNombre", usuario.Nombre);
                HttpContext.Session.SetString("UsuarioRol", usuario.Rol);
                HttpContext.Session.SetString("UsuarioMatricula", usuario.Matricula);
                HttpContext.Session.SetInt32("UsuarioIdCarrera", usuario.IdCarrera ?? 0);

                // Redirección basada en rol o carrera
                if (usuario.Rol == "Admin")
                {
                    return RedirectToAction("Admin", "Admin");
                }

                switch (usuario.IdCarrera)
                {
                    case 1:
                        return RedirectToAction("Tecnologias", "Tecnologias");
                    case 2:
                        return RedirectToAction("Negocios", "Negocios");
                    case 3:
                        return RedirectToAction("Mecatronica", "Mecatronica");
                    case 4:
                        return RedirectToAction("Energia", "Energia");
                    case 5:
                        return RedirectToAction("Biotecnologia", "Biotecnologia");
                    case 6:
                        return RedirectToAction("Industri", "Industri");
                    default:
                        TempData["ErrorMessage"] = "No tienes acceso a ningún perfil configurado.";
                        break;
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Limpia toda la sesión
            return RedirectToAction("Login");
        }
    }
}
