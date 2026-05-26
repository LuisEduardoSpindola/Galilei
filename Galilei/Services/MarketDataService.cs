using System.Text.Json;
using Galilei.Models.Dashboards;
using Galilei.Models.Market;

namespace Galilei.Services
{
    public class MarketDataService : IMarketDataService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<MarketDataService> _logger;

        public MarketDataService(HttpClient httpClient, ILogger<MarketDataService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<string>> GetB3TickersAsync()
        {
            await Task.Delay(100);
            var tickers = new List<string> 
            {
                "PETR4", "PETR3", "VALE3", "ITUB4", "BBDC4", "BBDC3", "BBAS3", "ABEV3", "WEGE3", "RENT3",
                "SUZB3", "BPAC11", "EQTL3", "RADL3", "B3SA3", "LREN3", "GGBR4", "HAPV3", "PRIO3", "RAIL3",
                "JBSS3", "SBSP3", "VIVT3", "CMIG4", "BBSE3", "CPLE6", "KLBN11", "ELET3", "ELET6", "CSAN3",
                "TIMS3", "CCRO3", "TOTS3", "EGIE3", "ENEV3", "CSNA3", "CYRE3", "YDUQ3", "MGLU3", "COGN3"
            };
            return tickers.OrderBy(t => t).ToList();
        }

        public async Task<List<AssetData>> GetMarketDataForTickersAsync(IEnumerable<string> tickers)
        {
            var result = new List<AssetData>();
            if (tickers == null || !tickers.Any()) return result;

            var distinctTickers = tickers.Distinct().ToList();
            var tasks = distinctTickers.Select(async ticker =>
            {
                try
                {
                    // Yahoo Finance API expects .SA for B3 stocks
                    string queryTicker = ticker.EndsWith(".SA", StringComparison.OrdinalIgnoreCase) ? ticker : $"{ticker}.SA";

                    var request = new HttpRequestMessage(HttpMethod.Get, $"https://query2.finance.yahoo.com/v8/finance/chart/{queryTicker}");
                    request.Headers.Add("User-Agent", "Mozilla/5.0");

                    var response = await _httpClient.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var jsonDoc = JsonDocument.Parse(content);
                        var resultElement = jsonDoc.RootElement.GetProperty("chart").GetProperty("result")[0];
                        var meta = resultElement.GetProperty("meta");

                        decimal regularMarketPrice = meta.GetProperty("regularMarketPrice").GetDecimal();
                        decimal previousClose = meta.GetProperty("chartPreviousClose").GetDecimal();

                        decimal change = 0;
                        if (previousClose > 0)
                        {
                            change = ((regularMarketPrice - previousClose) / previousClose) * 100;
                        }

                        string name = ticker;
                        if (meta.TryGetProperty("shortName", out var shortName) && shortName.ValueKind == JsonValueKind.String)
                        {
                            name = shortName.GetString() ?? ticker;
                        }

                        return new AssetData
                        {
                            Symbol = ticker,
                            Name = name,
                            Price = regularMarketPrice,
                            ChangePercentage24h = change
                        };
                    }
                    else
                    {
                        _logger.LogWarning("Failed to fetch real market data for {Ticker}. Status: {StatusCode}", ticker, response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error fetching market data from API for {Ticker}", ticker);
                }

                return new AssetData
                {
                    Symbol = ticker,
                    Name = ticker,
                    Price = 0,
                    ChangePercentage24h = 0
                };
            });

            var assetDataArray = await Task.WhenAll(tasks);
            return assetDataArray.ToList();
        }

        public async Task<List<NewsArticle>> GetFinancialNewsAsync()
        {
            try
            {
                // In a real scenario, integrate with NewsAPI, AlphaVantage, or similar.
                // Examples: https://newsapi.org/v2/top-headlines?category=business&apiKey=XYZ
                // HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://api.marketnews.example/v1/news");
                // var response = await _httpClient.SendAsync(request);
                // response.EnsureSuccessStatusCode();

                // Mocking professional data as fallback/default since we lack real keys
                await Task.Delay(200); // simulate network call
                return new List<NewsArticle>
                {
                    new NewsArticle
                    {
                        Title = "Mercado reage positivamente aos novos cortes de juros",
                        Summary = "O banco central anunciou nesta manhã novos cortes, impulsionando o mercado de ações globalmente.",
                        Source = "Valor Econômico",
                        PublishedAt = DateTime.Now.AddHours(-1),
                        ImageUrl = "https://images.unsplash.com/photo-1611974789855-9c2a0a7236a3?q=80&w=400&auto=format&fit=crop",
                        Url = "#"
                    },
                    new NewsArticle
                    {
                        Title = "Alta histórica do Bitcoin atrai investidores institucionais",
                        Summary = "Grandes fundos americanos estão movendo bilhões para ETFs de criptomoedas.",
                        Source = "CoinDesk",
                        PublishedAt = DateTime.Now.AddHours(-3),
                        ImageUrl = "https://images.unsplash.com/photo-1518546305927-5a555bb7020d?q=80&w=400&auto=format&fit=crop",
                        Url = "#"
                    },
                    new NewsArticle
                    {
                        Title = "Balanços corporativos do 3º trimestre superam expectativas",
                        Summary = "Gigantes da tecnologia mostram força com aumento de receita em serviços de IA e nuvem.",
                        Source = "InfoMoney",
                        PublishedAt = DateTime.Now.AddHours(-5),
                        ImageUrl = "https://images.unsplash.com/photo-1590283603385-17ffb3a7f29f?q=80&w=400&auto=format&fit=crop",
                        Url = "#"
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching news from API.");
                return new List<NewsArticle>();
            }
        }

        public async Task<List<AssetData>> GetBRStocksDataAsync()
        {
            var tickers = new List<string> { "PETR4", "VALE3", "ITUB4", "B3SA3" };
            return await GetMarketDataForTickersAsync(tickers);
        }

        public async Task<List<AssetData>> GetFixedIncomeDataAsync()
        {
            await Task.Delay(200);
            return new List<AssetData>
            {
                new AssetData { Symbol = "TD", Name = "Tesouro Direto Selic", Price = 14500.50m, ChangePercentage24h = 0.03m },
                new AssetData { Symbol = "IPCA+", Name = "Tesouro IPCA+ 2045", Price = 1250.70m, ChangePercentage24h = 0.15m },
                new AssetData { Symbol = "CDB", Name = "CDB Banco ABC 110% CDI", Price = 1000.00m, ChangePercentage24h = 0.03m },
                new AssetData { Symbol = "LCI", Name = "LCI Itaú 95% CDI", Price = 5000.00m, ChangePercentage24h = 0.03m },
            };
        }
    }
}
