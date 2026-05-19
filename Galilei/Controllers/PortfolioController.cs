using Galilei.Data;
using Galilei.Models;
using Galilei.Models.Portfolio;
using Galilei.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Galilei.Controllers
{
    [Authorize]
    public class PortfolioController : Controller
    {
        private readonly GalileiContext _context;
        private readonly IMarketDataService _marketService;
        private readonly IPriceAlertService _priceAlertService;

        public PortfolioController(GalileiContext context, IMarketDataService marketService, IPriceAlertService priceAlertService)
        {
            _context = context;
            _marketService = marketService;
            _priceAlertService = priceAlertService;
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

        public async Task<IActionResult> Index()
        {
            int userId = GetUserId();
            var userAssets = await _context.UserAssets
                .Where(a => a.UserId == userId)
                .OrderBy(a => a.Ticker)
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

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> AddAsset()
        {
            var tickers = await _marketService.GetB3TickersAsync();
            ViewBag.Tickers = tickers;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAsset(UserAsset model)
        {
            model.UserId = GetUserId();
            var tickers = await _marketService.GetB3TickersAsync();
            ViewBag.Tickers = tickers;

            // Simple validation check
            if (string.IsNullOrWhiteSpace(model.Ticker))
            {
                ModelState.AddModelError("Ticker", "O ticker é obrigatório.");
            }
            if (model.Quantity <= 0)
            {
                ModelState.AddModelError("Quantity", "A quantidade deve ser maior que zero.");
            }
            if (model.AveragePrice <= 0)
            {
                ModelState.AddModelError("AveragePrice", "O preço médio deve ser maior que zero.");
            }
            if (model.DesiredPrice <= 0)
            {
                ModelState.AddModelError("DesiredPrice", "O preço desejado deve ser maior que zero.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.Ticker = model.Ticker.ToUpper();

            // Verify if already exists to calculate new average price
            var existingAsset = await _context.UserAssets
                .FirstOrDefaultAsync(a => a.UserId == model.UserId && a.Ticker == model.Ticker);

            if (existingAsset != null)
            {
                var totalValue = (existingAsset.Quantity * existingAsset.AveragePrice) + (model.Quantity * model.AveragePrice);
                existingAsset.Quantity += model.Quantity;
                existingAsset.AveragePrice = totalValue / existingAsset.Quantity;
                existingAsset.DesiredPrice = model.DesiredPrice;
                existingAsset.DesiredPriceType = model.DesiredPriceType;
                existingAsset.IsTargetNotified = false;
                
                _context.Update(existingAsset);
            }
            else
            {
                model.IsTargetNotified = false;
                _context.UserAssets.Add(model);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveAsset(int id)
        {
            var userId = GetUserId();
            var asset = await _context.UserAssets.FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);
            
            if (asset != null)
            {
                _context.UserAssets.Remove(asset);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction("Index");
        }
    }
}
