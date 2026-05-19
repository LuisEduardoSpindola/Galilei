using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Galilei.Services;

namespace Galilei.Controllers
{
    [Authorize]
    public class MarketController : Controller
    {
        private readonly IMarketDataService _marketDataService;

        public MarketController(IMarketDataService marketDataService)
        {
            _marketDataService = marketDataService;
        }

        public async Task<IActionResult> News()
        {
            var news = await _marketDataService.GetFinancialNewsAsync();
            return View(news);
        }
    }
}
