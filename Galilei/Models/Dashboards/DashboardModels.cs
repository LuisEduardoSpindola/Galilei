namespace Galilei.Models.Dashboards
{
    public class AssetData
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Change24h { get; set; }
        public decimal ChangePercentage24h { get; set; }
        public decimal Volume { get; set; }
        public decimal MarketCap { get; set; }
    }
}
