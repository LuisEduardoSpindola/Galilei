using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Galilei.Services;

namespace Galilei.Controllers
{
    [Authorize]
    public class DashboardsController : Controller
    {
        private readonly IMarketDataService _marketDataService;

        public DashboardsController(IMarketDataService marketDataService)
        {
            _marketDataService = marketDataService;
        }

        public async Task<IActionResult> Crypto()
        {
            var data = await _marketDataService.GetCryptoDataAsync();
            ViewData["Title"] = "Dashboard - Criptomoedas";
            return View("GenericDashboard", data);
        }

        public async Task<IActionResult> USStocks()
        {
            var data = await _marketDataService.GetUSStocksDataAsync();
            ViewData["Title"] = "Dashboard - Ações Americanas";
            return View("GenericDashboard", data);
        }

        public async Task<IActionResult> BRStocks()
        {
            var data = await _marketDataService.GetBRStocksDataAsync();
            ViewData["Title"] = "Dashboard - Ações Brasileiras";
            return View("GenericDashboard", data);
        }

        public async Task<IActionResult> FixedIncome()
        {
            var data = await _marketDataService.GetFixedIncomeDataAsync();
            ViewData["Title"] = "Dashboard - Renda Fixa";
            return View("GenericDashboard", data);
        }
    }
}
