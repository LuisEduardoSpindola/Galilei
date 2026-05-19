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
            await Task.Delay(150); // simulate API call
            var random = new Random();
            var result = new List<AssetData>();

            foreach (var ticker in tickers.Distinct())
            {
                // Generate mock price around 50 based on hash of ticker to make it "stable" between reloads
                int seed = ticker.GetHashCode();
                var pseudoRandom = new Random(seed);
                decimal basePrice = pseudoRandom.Next(20, 150) + (decimal)pseudoRandom.NextDouble();

                // Random variation from -5% to +5% each call
                decimal variation = (decimal)((random.NextDouble() * 10) - 5);
                decimal currentPrice = basePrice * (1 + (variation / 100));

                result.Add(new AssetData
                {
                    Symbol = ticker,
                    Name = ticker + " S.A.",
                    Price = currentPrice,
                    ChangePercentage24h = variation
                });
            }

            return result;
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
            await Task.Delay(300);
            return new List<AssetData>
            {
                new AssetData { Symbol = "PETR4", Name = "Petrobras PN", Price = 41.50m, ChangePercentage24h = 0.8m, Volume = 80000000m },
                new AssetData { Symbol = "VALE3", Name = "Vale ON", Price = 62.10m, ChangePercentage24h = -1.5m, Volume = 30000000m },
                new AssetData { Symbol = "ITUB4", Name = "Itaú Unibanco PN", Price = 33.40m, ChangePercentage24h = 1.2m, Volume = 45000000m },
                new AssetData { Symbol = "B3SA3", Name = "B3 SA", Price = 12.80m, ChangePercentage24h = 0.2m, Volume = 25000000m },
            };
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
