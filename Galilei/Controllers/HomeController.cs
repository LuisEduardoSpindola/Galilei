using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Galilei.Models;

namespace Galilei.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View("Dashboard");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("Home/Error/{statusCode?}")]
        [AllowAnonymous]
        public IActionResult Error(int? statusCode)
        {
            if (statusCode.HasValue)
            {
                if (statusCode == 404)
                {
                    ViewData["Title"] = "Página Não Encontrada";
                    ViewData["ErrorMessage"] = "A página que você procura pode ter sido removida ou não existe.";
                    ViewData["ErrorCode"] = "404";
                }
                else if (statusCode == 403 || statusCode == 401)
                {
                    ViewData["Title"] = "Acesso Negado";
                    ViewData["ErrorMessage"] = "Você não tem permissão para acessar este recurso.";
                    ViewData["ErrorCode"] = "403";
                }
                else
                {
                    ViewData["Title"] = "Erro";
                    ViewData["ErrorMessage"] = "Ocorreu um erro inesperado. Tente novamente mais tarde.";
                    ViewData["ErrorCode"] = statusCode.ToString();
                }
            }
            else
            {
                ViewData["Title"] = "Erro";
                ViewData["ErrorMessage"] = "Ocorreu um erro ao processar sua solicitação.";
                ViewData["ErrorCode"] = "Erro";
            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
