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

        public async Task<List<PlatformRecommendation>> GetPlatformsAsync()
        {
            await Task.Delay(100);
            return new List<PlatformRecommendation>
            {
                new PlatformRecommendation
                {
                    Name = "XP Investimentos",
                    Category = "Corretora",
                    Rating = "4.8/5",
                    Description = "Maior ecossistema de investimentos do Brasil, excelente para renda fixa e variável.",
                    Link = "https://xpi.com.br",
                    ImageUrl = "https://images.unsplash.com/photo-1560472355-536de3962603?q=80&w=400&auto=format&fit=crop"
                },
                new PlatformRecommendation
                {
                    Name = "Avenue",
                    Category = "Corretora Internacional",
                    Rating = "4.7/5",
                    Description = "Plataforma focada em investidores brasileiros que desejam acessar o mercado americano de forma simplificada.",
                    Link = "https://avenue.us",
                    ImageUrl = "https://images.unsplash.com/photo-1642543492481-44e81e3914a7?q=80&w=400&auto=format&fit=crop"
                },
                new PlatformRecommendation
                {
                    Name = "Suno Educação",
                    Category = "Educação Financeira",
                    Rating = "4.9/5",
                    Description = "Análises independentes, relatórios e cursos para formar investidores de sucesso a longo prazo.",
                    Link = "https://sunoresearch.com.br",
                    ImageUrl = "https://images.unsplash.com/photo-1509062522246-3755977927d7?q=80&w=400&auto=format&fit=crop"
                }
            };
        }

        public async Task<List<AssetData>> GetCryptoDataAsync()
        {
            try 
            {
                // Can integrate with CoinGecko Public API here: https://api.coingecko.com/api/v3/coins/markets?vs_currency=brl
                // _httpClient.BaseAddress = new Uri("https://api.coingecko.com/api/v3/");
                await Task.Delay(300);
                return new List<AssetData>
                {
                    new AssetData { Symbol = "BTC", Name = "Bitcoin", Price = 345000m, ChangePercentage24h = 2.5m, Volume = 50000000000m },
                    new AssetData { Symbol = "ETH", Name = "Ethereum", Price = 18500m, ChangePercentage24h = -1.2m, Volume = 20000000000m },
                    new AssetData { Symbol = "SOL", Name = "Solana", Price = 800m, ChangePercentage24h = 5.7m, Volume = 5000000000m },
                    new AssetData { Symbol = "ADA", Name = "Cardano", Price = 3.5m, ChangePercentage24h = 0.5m, Volume = 400000000m },
                };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Crypto api failed.");
                return new List<AssetData>();
            }
        }

        public async Task<List<AssetData>> GetUSStocksDataAsync()
        {
            await Task.Delay(300);
            return new List<AssetData>
            {
                new AssetData { Symbol = "AAPL", Name = "Apple Inc.", Price = 185.92m, ChangePercentage24h = 1.1m, Volume = 60000000m },
                new AssetData { Symbol = "MSFT", Name = "Microsoft Corp.", Price = 398.21m, ChangePercentage24h = 0.4m, Volume = 25000000m },
                new AssetData { Symbol = "GOOG", Name = "Alphabet Inc.", Price = 145.22m, ChangePercentage24h = -0.5m, Volume = 15000000m },
                new AssetData { Symbol = "TSLA", Name = "Tesla Inc.", Price = 202.64m, ChangePercentage24h = 3.2m, Volume = 35000000m },
            };
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
