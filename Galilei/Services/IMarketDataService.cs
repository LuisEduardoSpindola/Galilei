using Galilei.Models.Market;
using Galilei.Models.Dashboards;

namespace Galilei.Services
{
    public interface IMarketDataService
    {
        Task<List<NewsArticle>> GetFinancialNewsAsync();
        Task<List<PlatformRecommendation>> GetPlatformsAsync();
        Task<List<AssetData>> GetCryptoDataAsync();
        Task<List<AssetData>> GetUSStocksDataAsync();
        Task<List<AssetData>> GetBRStocksDataAsync();
        Task<List<AssetData>> GetFixedIncomeDataAsync();
    }
}
