using System.Text.Json;
using System.Text.Json.Serialization;

namespace EndReinigung.Models
{
    public sealed class RatgeberFaq
    {
        [JsonPropertyName("q")]
        public string Q { get; set; } = "";

        [JsonPropertyName("a")]
        public string A { get; set; } = "";
    }

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

        [JsonPropertyName("faqs")]
        public List<RatgeberFaq> Faqs { get; set; } = new();
    }

    public sealed class RatgeberHowToStep
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("text")]
        public string Text { get; set; } = "";
    }

    public sealed class RatgeberHowTo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("description")]
        public string Description { get; set; } = "";

        [JsonPropertyName("totalTime")]
        public string TotalTime { get; set; } = "";

        [JsonPropertyName("steps")]
        public List<RatgeberHowToStep> Steps { get; set; } = new();
    }

    public sealed class RatgeberArticle
    {
        [JsonPropertyName("slug")]
        public string Slug { get; set; } = "";

        [JsonPropertyName("enSlug")]
        public string EnSlug { get; set; } = "";

        [JsonPropertyName("categorySlug")]
        public string CategorySlug { get; set; } = "";

        [JsonPropertyName("categoryName")]
        public string CategoryName { get; set; } = "";

        [JsonPropertyName("categoryNameEn")]
        public string CategoryNameEn { get; set; } = "";

        [JsonPropertyName("titleDe")]
        public string TitleDe { get; set; } = "";

        [JsonPropertyName("titleEn")]
        public string TitleEn { get; set; } = "";

        [JsonPropertyName("introDe")]
        public string IntroDe { get; set; } = "";

        [JsonPropertyName("introEn")]
        public string IntroEn { get; set; } = "";

        [JsonPropertyName("metaTitleDe")]
        public string MetaTitleDe { get; set; } = "";

        [JsonPropertyName("metaTitleEn")]
        public string MetaTitleEn { get; set; } = "";

        [JsonPropertyName("metaDescriptionDe")]
        public string MetaDescriptionDe { get; set; } = "";

        [JsonPropertyName("metaDescriptionEn")]
        public string MetaDescriptionEn { get; set; } = "";

        [JsonPropertyName("authorBylineDe")]
        public string AuthorBylineDe { get; set; } = "";

        [JsonPropertyName("authorBylineEn")]
        public string AuthorBylineEn { get; set; } = "";

        [JsonPropertyName("heroAltDe")]
        public string HeroAltDe { get; set; } = "";

        [JsonPropertyName("heroAltEn")]
        public string HeroAltEn { get; set; } = "";

        [JsonPropertyName("dateModified")]
        public string DateModified { get; set; } = "";

        [JsonPropertyName("datePublished")]
        public string DatePublished { get; set; } = "";

        [JsonPropertyName("readingTimeMinutes")]
        public int? ReadingTimeMinutes { get; set; }

        [JsonPropertyName("coverImage")]
        public string CoverImage { get; set; } = "";

        [JsonPropertyName("blocks")]
        public List<RatgeberBlock> Blocks { get; set; } = new();

        [JsonPropertyName("blocksEn")]
        public List<RatgeberBlock> BlocksEn { get; set; } = new();

        [JsonPropertyName("howToDe")]
        public RatgeberHowTo? HowToDe { get; set; }

        [JsonPropertyName("howToEn")]
        public RatgeberHowTo? HowToEn { get; set; }
    }

    public static class RatgeberArticleData
    {
        private static readonly Lazy<IReadOnlyList<RatgeberArticle>> _all = new(LoadFromDisk);

        public static IReadOnlyList<RatgeberArticle> All => _all.Value;

        public static RatgeberArticle? FindBySlug(string slug)
            => All.FirstOrDefault(a => string.Equals(a.Slug, slug, StringComparison.OrdinalIgnoreCase));

        public static RatgeberArticle? FindByEnSlug(string enSlug)
            => All.FirstOrDefault(a => !string.IsNullOrEmpty(a.EnSlug)
                && string.Equals(a.EnSlug, enSlug, StringComparison.OrdinalIgnoreCase));

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
