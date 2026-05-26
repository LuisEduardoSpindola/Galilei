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

        public async Task<IActionResult> BRStocks()
        {
            var data = await _marketDataService.GetBRStocksDataAsync();
            ViewData["Title"] = "Home Broker - Ações B3";
            return View(data);
        }

        public async Task<IActionResult> FixedIncome()
        {
            var data = await _marketDataService.GetFixedIncomeDataAsync();
            ViewData["Title"] = "Dashboard - Renda Fixa";
            return View(data);
        }
    }
}
