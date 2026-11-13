namespace EndReinigung.Models
{
    /// <summary>
    /// The single source of truth for which German page has an English counterpart.
    /// Used by the layout (hreflang), the DE/EN switcher, the sitemap check and in-page links.
    ///
    /// Language is decided by the URL only: everything under /en is English, everything else is
    /// German (see Program.cs). A page without an entry here has no English version.
    /// When you add an English page: add its route in HomeController and one line below.
    /// </summary>
    public static class LanguageRoutes
    {
        public const string SiteUrl = "https://zurich-endreinigung.ch";
        public const string EnHome = "/en";

        // German path -> English path. Ratgeber articles are added from ratgeber-articles.json (EnSlug).
        private static readonly (string De, string En)[] StaticPairs =
        {
            ("/", "/en"),
            ("/dienstleistungen", "/en/services"),
            ("/ratgeber", "/en/guide"),
            ("/airbnb-reinigung-zuerich", "/en/airbnb-cleaning-zurich"),
            ("/polsterreinigung", "/en/upholstery-cleaning-zurich"),
        };

        private static readonly Lazy<(Dictionary<string, string> DeToEn, Dictionary<string, string> EnToDe)> Maps = new(() =>
        {
            var deToEn = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var (de, en) in StaticPairs) deToEn[de] = en;
            foreach (var a in RatgeberArticleData.All.Where(a => !string.IsNullOrEmpty(a.EnSlug)))
                deToEn[$"/ratgeber/{a.Slug}"] = $"/en/guide/{a.EnSlug}";
            var enToDe = deToEn.ToDictionary(kv => kv.Value, kv => kv.Key, StringComparer.OrdinalIgnoreCase);
            return (deToEn, enToDe);
        });

        /// <summary>All German/English pairs.</summary>
        public static IEnumerable<KeyValuePair<string, string>> Pairs => Maps.Value.DeToEn;

        public static bool IsEnglishPath(string? path) =>
            !string.IsNullOrEmpty(path) &&
            (path.Equals("/en", StringComparison.OrdinalIgnoreCase) || path.StartsWith("/en/", StringComparison.OrdinalIgnoreCase));

        /// <summary>Strips the query/fragment and a trailing slash ("/en/" -> "/en").</summary>
        public static string Normalize(string? path)
        {
            if (string.IsNullOrEmpty(path)) return "/";
            var cut = path.IndexOfAny(new[] { '?', '#' });
            if (cut >= 0) path = path.Substring(0, cut);
            return path.Length > 1 ? path.TrimEnd('/') : path;
        }

        /// <summary>
        /// Returns the (German, English) pair the path belongs to, or null when the page exists in one
        /// language only.
        /// </summary>
        public static (string De, string En)? FindPair(string? path)
        {
            var p = Normalize(path);
            if (Maps.Value.DeToEn.TryGetValue(p, out var en)) return (p, en);
            if (Maps.Value.EnToDe.TryGetValue(p, out var de)) return (de, p);
            return null;
        }

        /// <summary>
        /// Rewrites an internal German link for the current language: on an English page, a link to a
        /// German page that has an English version points to that version. Fragments are kept
        /// ("/#pakete" -> "/en#pakete"). Everything else is returned unchanged.
        /// </summary>
        public static string Localize(string href, bool isEn)
        {
            if (!isEn || string.IsNullOrEmpty(href) || !href.StartsWith('/') || href.StartsWith("//")) return href;
            var cut = href.IndexOfAny(new[] { '?', '#' });
            var path = cut >= 0 ? href.Substring(0, cut) : href;
            var rest = cut >= 0 ? href.Substring(cut) : "";
            return Maps.Value.DeToEn.TryGetValue(Normalize(path), out var en) ? en + rest : href;
        }
    }
}
