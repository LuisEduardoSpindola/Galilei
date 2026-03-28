namespace Galilei.Models.Market
{
    public class NewsArticle
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Url { get; set; }
        public string Source { get; set; }
        public string ImageUrl { get; set; }
        public DateTime PublishedAt { get; set; }
    }

    public class PlatformRecommendation
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Rating { get; set; }
        public string Link { get; set; }
        public string ImageUrl { get; set; }
    }
}
