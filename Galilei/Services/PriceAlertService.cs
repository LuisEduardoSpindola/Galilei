using Galilei.Data;
using Galilei.Models;
using Galilei.Models.Dashboards;
using Microsoft.EntityFrameworkCore;

namespace Galilei.Services
{
    public class PriceAlertService : IPriceAlertService
    {
        private readonly GalileiContext _context;
        private readonly IEmailService _emailService;
        private readonly ILogger<PriceAlertService> _logger;

        public PriceAlertService(GalileiContext context, IEmailService emailService, ILogger<PriceAlertService> logger)
        {
            _context = context;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task CheckAndSendAsync(int userId, IReadOnlyCollection<UserAsset> assets, IReadOnlyCollection<AssetData> marketData)
        {
            if (assets.Count == 0 || marketData.Count == 0)
            {
                return;
            }

            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null || string.IsNullOrWhiteSpace(user.Email))
            {
                return;
            }

            var shouldSave = false;

            foreach (var asset in assets)
            {
                if (asset.IsTargetNotified || !asset.DesiredPrice.HasValue || asset.DesiredPrice.Value <= 0 || asset.DesiredPriceType == DesiredPriceType.Nenhum)
                {
                    continue;
                }

                var marketInfo = marketData.FirstOrDefault(m => m.Symbol == asset.Ticker);
                if (marketInfo == null)
                {
                    continue;
                }

                var reached = asset.DesiredPriceType == DesiredPriceType.Compra
                    ? marketInfo.Price <= asset.DesiredPrice.Value
                    : marketInfo.Price >= asset.DesiredPrice.Value;

                if (!reached)
                {
                    continue;
                }

                var direction = asset.DesiredPriceType == DesiredPriceType.Compra ? "compra" : "venda";
                var subject = $"Alerta de preço para {asset.Ticker}";
                var body = $"Olá {user.FullName},\n\nO ativo {asset.Ticker} atingiu o preço desejado para {direction}.\nPreço desejado: R$ {asset.DesiredPrice.Value:N2}\nPreço atual: R$ {marketInfo.Price:N2}\n\nAcesse sua carteira no Galilei para mais detalhes.";

                var sent = await _emailService.SendAsync(user.Email, subject, body);
                if (sent)
                {
                    asset.IsTargetNotified = true;
                    shouldSave = true;
                }
            }

            if (shouldSave)
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to update price alert flags.");
                }
            }
        }
    }
}
