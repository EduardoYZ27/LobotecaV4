using Microsoft.AspNetCore.Mvc;

namespace Loboteca_v2.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Admin()
        {
            return View();
        }
    }
}
