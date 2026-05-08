using EndReinigung.Models;

namespace EndReinigung.Services
{
    /// <summary>
    /// Authoritative server-side price calculation. Keep in sync with wwwroot/js/calculator.js.
    /// </summary>
    public static class PriceCalculator
    {
        private static readonly Dictionary<string, decimal> ApartmentPrices = new()
        {
            { "1", 500 }, { "1.5", 550 }, { "2", 600 }, { "2.5", 650 },
            { "3", 800 }, { "3.5", 850 }, { "4", 920 }, { "4.5", 970 },
            { "5", 1100 }, { "5.5", 1150 }, { "6", 1300 }, { "6.5", 1350 },
        };

        private static readonly Dictionary<string, decimal> HousePrices = new()
        {
            { "3", 1299 }, { "3.5", 1799 }, { "4", 1599 }, { "4.5", 2099 },
            { "5", 1899 }, { "5.5", 2399 }, { "6", 2199 }, { "6.5", 2699 },
            { "7", 2399 }, { "7.5", 2899 }, { "8", 2599 }, { "8.5", 3099 },
        };

        public const decimal PriceBasement = 50m;
        public const decimal PriceBalcony = 45m;
        public const decimal PriceUtilityBalcony = 35m;
        public const decimal PriceBath = 65m;
        public const decimal PriceWc = 40m;
        public const decimal PriceCarpet = 120m;
        public const decimal PriceBalconyPressure = 50m;
        public const decimal PriceGaragePressure = 65m;

        public static decimal Compute(BookingViewModel b)
        {
            var table = b.PropertyType == "house" ? HousePrices : ApartmentPrices;
            if (!table.TryGetValue(b.RoomSize, out var basePrice))
            {
                return 0m;
            }

            decimal extras = 0m;
            if (b.Basement) extras += PriceBasement;
            extras += b.Balcony * PriceBalcony;
            extras += b.UtilityBalcony * PriceUtilityBalcony;
            extras += b.Bath * PriceBath;
            extras += b.Wc * PriceWc;
            extras += b.Carpet * PriceCarpet;
            extras += b.BalconyPressure * PriceBalconyPressure;
            extras += b.GaragePressure * PriceGaragePressure;

            return basePrice + extras;
        }
    }
}
