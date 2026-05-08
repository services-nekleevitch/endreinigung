using System.Globalization;

namespace EndReinigung.Models
{
    public sealed record BauPrice(string Size, int Standard, string Area, bool Popular = false);

    public static class BaureinigungPricing
    {
        public static readonly IReadOnlyList<BauPrice> Wohnung = new[]
        {
            new BauPrice("1-1.5 Zimmer", 435,  "Bis ca. 50m²"),
            new BauPrice("2-2.5 Zimmer", 565,  "Bis ca. 75m²"),
            new BauPrice("3-3.5 Zimmer", 725,  "Bis ca. 100m²", Popular: true),
            new BauPrice("4-4.5 Zimmer", 895,  "Bis ca. 125m²"),
            new BauPrice("5-5.5 Zimmer", 1095, "Bis ca. 150m²"),
        };

        public static readonly IReadOnlyList<BauPrice> Haus = new[]
        {
            new BauPrice("3-3.5 Zimmer", 1289, "Bis ca. 130m²"),
            new BauPrice("4-4.5 Zimmer", 1589, "Bis ca. 170m²"),
            new BauPrice("5-5.5 Zimmer", 1889, "Bis ca. 210m²"),
        };

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
