using System.Globalization;

namespace EndReinigung.Models
{
    public sealed record BauPrice(string Size, int Standard, string Area, bool Popular = false);

    public sealed record BauRoomSize(string Value, string Label, int Price);

    public static class BaureinigungPricing
    {
        // Per-half-step pricing — used by the calculator. Keep in sync with
        // rich-lease-shine/src/lib/baureinigungPricing.ts and wwwroot/js/baureinigung-calculator.js.
        public static readonly IReadOnlyList<BauRoomSize> WohnungRoomSizes = new[]
        {
            new BauRoomSize("1",   "1 Zimmer",   435),
            new BauRoomSize("1.5", "1.5 Zimmer", 449),
            new BauRoomSize("2",   "2 Zimmer",   565),
            new BauRoomSize("2.5", "2.5 Zimmer", 595),
            new BauRoomSize("3",   "3 Zimmer",   725),
            new BauRoomSize("3.5", "3.5 Zimmer", 745),
            new BauRoomSize("4",   "4 Zimmer",   895),
            new BauRoomSize("4.5", "4.5 Zimmer", 945),
            new BauRoomSize("5",   "5 Zimmer",   1095),
            new BauRoomSize("5.5", "5.5 Zimmer", 1150),
        };

        public static readonly IReadOnlyList<BauRoomSize> HausRoomSizes = new[]
        {
            new BauRoomSize("3",   "3 Zimmer",   1289),
            new BauRoomSize("3.5", "3.5 Zimmer", 1389),
            new BauRoomSize("4",   "4 Zimmer",   1589),
            new BauRoomSize("4.5", "4.5 Zimmer", 1689),
            new BauRoomSize("5",   "5 Zimmer",   1889),
            new BauRoomSize("5.5", "5.5 Zimmer", 1989),
        };

        // Grouped tiers — used by the price cards. Derived from the per-half-step tables
        // so cards and calculator can never diverge.
        public static readonly IReadOnlyList<BauPrice> Wohnung = new[]
        {
            new BauPrice("1-1.5 Zimmer", WohnungRoomSizes[0].Price, "Bis ca. 50m²"),
            new BauPrice("2-2.5 Zimmer", WohnungRoomSizes[2].Price, "Bis ca. 75m²"),
            new BauPrice("3-3.5 Zimmer", WohnungRoomSizes[4].Price, "Bis ca. 100m²", Popular: true),
            new BauPrice("4-4.5 Zimmer", WohnungRoomSizes[6].Price, "Bis ca. 125m²"),
            new BauPrice("5-5.5 Zimmer", WohnungRoomSizes[8].Price, "Bis ca. 150m²"),
        };

        public static readonly IReadOnlyList<BauPrice> Haus = new[]
        {
            new BauPrice("3-3.5 Zimmer", HausRoomSizes[0].Price, "Bis ca. 130m²"),
            new BauPrice("4-4.5 Zimmer", HausRoomSizes[2].Price, "Bis ca. 170m²"),
            new BauPrice("5-5.5 Zimmer", HausRoomSizes[4].Price, "Bis ca. 210m²"),
        };

        // Baureinigung-specific add-on pricing.
        public const decimal PriceBauschutt = 120m;
        public const decimal PriceFassade = 180m;

        public static readonly IReadOnlyList<string> IncludedServices = new[]
        {
            "Baustaub komplett entfernen",
            "Grobschmutz und Bauschutt beseitigen",
            "Fenster innen & aussen reinigen",
            "Böden schleifen und wischen",
            "Sanitäranlagen reinigen",
            "Feinreinigung aller Oberflächen",
            "Küchengeräte reinigen",
        };

        public static int WohnungStartingPrice => Wohnung[0].Standard;
        public static int HausStartingPrice => Haus[0].Standard;

        // Swiss-German thousands format: 1'095 (apostrophe).
        public static string FormatChf(int n) =>
            n.ToString("N0", CultureInfo.GetCultureInfo("de-CH")).Replace('’', '\'');
    }
}
