using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Galilei.Models;
using Galilei.Data;
using Galilei.Services;
using Galilei.Models.Portfolio;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Galilei.Controllers
{
    public class HomeController : Controller
    {
        private readonly GalileiContext _context;
        private readonly IMarketDataService _marketService;
        private readonly IPriceAlertService _priceAlertService;

        public HomeController(GalileiContext context, IMarketDataService marketService, IPriceAlertService priceAlertService)
        {
            _context = context;
            _marketService = marketService;
            _priceAlertService = priceAlertService;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var userAssets = await _context.UserAssets
                    .Where(a => a.UserId == userId)
                    .ToListAsync();

                var tickers = userAssets.Select(a => a.Ticker).Distinct().ToList();
                var marketData = await _marketService.GetMarketDataForTickersAsync(tickers);

                await _priceAlertService.CheckAndSendAsync(userId, userAssets, marketData);

                var viewModel = new PortfolioViewModel();

                foreach (var asset in userAssets)
                {
                    var marketInfo = marketData.FirstOrDefault(m => m.Symbol == asset.Ticker);
                    decimal currentPrice = marketInfo?.Price ?? asset.AveragePrice;
                    decimal change24h = marketInfo?.ChangePercentage24h ?? 0;

                    viewModel.Assets.Add(new AssetItemViewModel
                    {
                        Id = asset.Id,
                        Ticker = asset.Ticker,
                        Quantity = asset.Quantity,
                        AveragePrice = asset.AveragePrice,
                        DesiredPrice = asset.DesiredPrice,
                        DesiredPriceType = asset.DesiredPriceType,
                        IsTargetNotified = asset.IsTargetNotified,
                        CurrentPrice = currentPrice,
                        ChangePercentage24h = change24h
                    });
                }

                return View("Dashboard", viewModel);
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
