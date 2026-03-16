using Microsoft.AspNetCore.Mvc;

namespace Galilei.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Will implement later
            return View();
        }
    }
}
