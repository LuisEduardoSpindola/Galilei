using Galilei.Models.Market;
using Galilei.Models.Dashboards;

namespace Galilei.Services
{
    public interface IMarketDataService
    {
        Task<List<NewsArticle>> GetFinancialNewsAsync();
        Task<List<AssetData>> GetBRStocksDataAsync();
        Task<List<AssetData>> GetFixedIncomeDataAsync();
        Task<List<string>> GetB3TickersAsync();
        Task<List<AssetData>> GetMarketDataForTickersAsync(IEnumerable<string> tickers);
    }
}
