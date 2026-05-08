using System.Text.Json;
using System.Text.Json.Serialization;

namespace EndReinigung.Models
{
    public sealed class RatgeberBlock
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "";

        [JsonPropertyName("heading")]
        public string Heading { get; set; } = "";

        [JsonPropertyName("body")]
        public string Body { get; set; } = "";

        [JsonPropertyName("items")]
        public List<string> Items { get; set; } = new();
    }

    public sealed class RatgeberArticle
    {
        [JsonPropertyName("slug")]
        public string Slug { get; set; } = "";

        [JsonPropertyName("categorySlug")]
        public string CategorySlug { get; set; } = "";

        [JsonPropertyName("categoryName")]
        public string CategoryName { get; set; } = "";

        [JsonPropertyName("titleDe")]
        public string TitleDe { get; set; } = "";

        [JsonPropertyName("introDe")]
        public string IntroDe { get; set; } = "";

        [JsonPropertyName("coverImage")]
        public string CoverImage { get; set; } = "";

        [JsonPropertyName("blocks")]
        public List<RatgeberBlock> Blocks { get; set; } = new();
    }

    public static class RatgeberArticleData
    {
        private static readonly Lazy<IReadOnlyList<RatgeberArticle>> _all = new(LoadFromDisk);

        public static IReadOnlyList<RatgeberArticle> All => _all.Value;

        public static RatgeberArticle? FindBySlug(string slug)
            => All.FirstOrDefault(a => string.Equals(a.Slug, slug, StringComparison.OrdinalIgnoreCase));

        private static IReadOnlyList<RatgeberArticle> LoadFromDisk()
        {
            var path = Path.Combine(AppContext.BaseDirectory, "App_Data", "ratgeber-articles.json");
            if (!File.Exists(path))
            {
                // Fall back to content root (App_Data is copied via csproj)
                path = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "ratgeber-articles.json");
            }
            using var stream = File.OpenRead(path);
            var articles = JsonSerializer.Deserialize<List<RatgeberArticle>>(stream)
                ?? new List<RatgeberArticle>();
            return articles;
        }
    }
}
