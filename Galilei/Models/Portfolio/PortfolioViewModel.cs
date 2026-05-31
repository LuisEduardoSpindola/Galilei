using Galilei.Models;

namespace Galilei.Models.Portfolio
{
    public class PortfolioViewModel
    {
        public List<AssetItemViewModel> Assets { get; set; } = new List<AssetItemViewModel>();
        public int AssetsCount => Assets.Count;
        public decimal TotalInvested => Assets.Sum(a => a.TotalInvested);
        public decimal CurrentBalance => Assets.Sum(a => a.CurrentTotal);
        public decimal TotalProfitLoss => CurrentBalance - TotalInvested;
        public decimal TotalProfitLossPercentage => TotalInvested > 0 ? (TotalProfitLoss / TotalInvested) * 100 : 0;
    }

    public class AssetItemViewModel
    {
        public int Id { get; set; }
        public string Ticker { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal TotalInvested => Quantity * AveragePrice;
        public decimal? DesiredPrice { get; set; }
        public DesiredPriceType DesiredPriceType { get; set; }
        public bool IsTargetNotified { get; set; }
        
        // Market Data
        public decimal CurrentPrice { get; set; }
        public decimal ChangePercentage24h { get; set; }
        
        // Calculated
        public decimal CurrentTotal => Quantity * CurrentPrice;
        public decimal ProfitLoss => CurrentTotal - TotalInvested;
        public decimal ProfitLossPercentage => TotalInvested > 0 ? (ProfitLoss / TotalInvested) * 100 : 0;
    }
}
