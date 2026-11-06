using System.Text.Json;
using System.Text.Json.Serialization;

namespace EndReinigung.Models
{
    public sealed class GoogleReview
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = "";

        [JsonPropertyName("authorName")]
        public string AuthorName { get; set; } = "";

        [JsonPropertyName("rating")]
        public int Rating { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; } = "";

        /// <summary>Unix seconds, as stored by Google / Lovable's google_reviews table.</summary>
        [JsonPropertyName("reviewTime")]
        public long ReviewTime { get; set; }

        [JsonPropertyName("profilePhotoUrl")]
        public string? ProfilePhotoUrl { get; set; }

        /// <summary>German relative age ("vor 4 Tagen"), computed at render time so a snapshot never goes stale.</summary>
        public string RelativeTimeDe
        {
            get
            {
                var days = (int)(DateTimeOffset.UtcNow - DateTimeOffset.FromUnixTimeSeconds(ReviewTime)).TotalDays;
                if (days < 1) return "heute";
                if (days < 7) return days == 1 ? "vor 1 Tag" : $"vor {days} Tagen";
                if (days < 30) { var w = days / 7; return w == 1 ? "vor 1 Woche" : $"vor {w} Wochen"; }
                if (days < 365) { var m = days / 30; return m == 1 ? "vor 1 Monat" : $"vor {m} Monaten"; }
                var y = days / 365;
                return y == 1 ? "vor 1 Jahr" : $"vor {y} Jahren";
            }
        }

        // Same palette + hash as rich-lease-shine GoogleReviewsWidget.tsx, so avatars get the same colours.
        private static readonly string[] AvatarColors = { "#1a73e8", "#7b1fa2", "#00897b", "#e8710a", "#c2185b", "#0288d1", "#5f6368" };

        public string AvatarColor
        {
            get
            {
                var hash = 0;
                foreach (var c in AuthorName) hash = (hash * 31 + c) % 997;
                return AvatarColors[hash % AvatarColors.Length];
            }
        }

        public string Initial => string.IsNullOrEmpty(AuthorName) ? "?" : AuthorName[..1].ToUpperInvariant();
    }

    public sealed class GoogleReviewsSnapshot
    {
        [JsonPropertyName("placeId")]
        public string PlaceId { get; set; } = "";

        [JsonPropertyName("rating")]
        public double Rating { get; set; } = 5.0;

        [JsonPropertyName("snapshotDate")]
        public string SnapshotDate { get; set; } = "";

        [JsonPropertyName("reviews")]
        public List<GoogleReview> Reviews { get; set; } = new();
    }

    /// <summary>
    /// Google reviews snapshot copied from Lovable's Supabase google_reviews table
    /// (see the lovable-migration skill). Loaded once at startup, like RatgeberArticleData.
    /// </summary>
    public static class GoogleReviewData
    {
        private static readonly Lazy<GoogleReviewsSnapshot> _snapshot = new(LoadFromDisk);

        public static GoogleReviewsSnapshot Snapshot => _snapshot.Value;

        public static IReadOnlyList<GoogleReview> Reviews => Snapshot.Reviews;

        public static string ReviewsUrl => $"https://search.google.com/local/reviews?place_id={Snapshot.PlaceId}";

        private static GoogleReviewsSnapshot LoadFromDisk()
        {
            var path = Path.Combine(AppContext.BaseDirectory, "App_Data", "google-reviews.json");
            if (!File.Exists(path))
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "google-reviews.json");
            }
            using var stream = File.OpenRead(path);
            var snapshot = JsonSerializer.Deserialize<GoogleReviewsSnapshot>(stream) ?? new GoogleReviewsSnapshot();
            snapshot.Reviews = snapshot.Reviews.OrderByDescending(r => r.ReviewTime).ToList();
            return snapshot;
        }
    }
}
