using App.Shared;
using AppServices.Models;
using DbAccess;
using DbAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace AppServices
{
    public class PriceCalculatorService : IPriceCalculatorService
    {
        private readonly AppDbContext _context;

        public PriceCalculatorService(AppDbContext context)
        {
            _context = context;
        }

        public decimal CalculateTotal(PropertyConfigurationModel config, int? priceSetId = null)
        {
            if (priceSetId == null)
                priceSetId = _context.PriceConfigurationEntries.Max(c => c.PriceConfigurationSetId);

            var priceEntries = _context.PriceConfigurationEntries
                .Where(e => e.PriceConfigurationSetId == priceSetId)
                .ToList();

            decimal total = 0;

            total += GetPrice(priceEntries, nameof(RoomCount), config.Rooms?.ToString());
            total += GetPrice(priceEntries, nameof(BathroomCount), config.Bathrooms?.ToString());
            total += GetPrice(priceEntries, nameof(PollutionLevel), config.PollutionLevel?.ToString());
            total += GetPrice(priceEntries, nameof(WindowType), config.WindowType?.ToString());
            total += GetPrice(priceEntries, nameof(FloorType), config.FloorType?.ToString());

            if (config.OtherRooms.HasValue)
                total += GetPrice(priceEntries, nameof(OtherRoomType), config.OtherRooms.Value.ToString());

            if (config.TerraceType.HasValue)
                total += GetPrice(priceEntries, nameof(TerraceType), config.TerraceType.Value.ToString());

            if (config.WinterGardenType.HasValue)
                total += GetPrice(priceEntries, nameof(WinterGardenType), config.WinterGardenType.Value.ToString());

            if (config.WithGuarantee)
                total += GetPrice(priceEntries, "Guarantee", "WithGuarantee");

            return total;
        }

        private decimal GetPrice(List<PriceConfigurationEntry> entries, string category, string key)
        {
            if (string.IsNullOrEmpty(key)) return 0;

            var price = entries
                .FirstOrDefault(e => e.Category == category && e.OptionKey == key);

            return price?.Price ?? 0;
        }
    }

    //public class PriceCalculatorService : IPriceCalculatorService
    //{
    //    private readonly AppDbContext _context;

    //    public PriceCalculatorService(AppDbContext context)
    //    {
    //        _context = context;
    //    }



    //    public async Task<decimal> CalculateTotalAsync(PropertyConfigurationViewModel config, int priceSetId)
    //    {
    //        var priceEntries = await _context.PriceConfigurationEntries
    //            .Where(e => e.PriceConfigurationSetId == priceSetId)
    //            .ToListAsync();

    //        decimal total = 0;

    //        total += GetPrice(priceEntries, nameof(RoomCount), config.Rooms?.ToString());
    //        total += GetPrice(priceEntries, nameof(BathroomCount), config.Bathrooms?.ToString());
    //        total += GetPrice(priceEntries, nameof(PollutionLevel), config.PollutionLevel?.ToString());
    //        total += GetPrice(priceEntries, nameof(WindowType), config.WindowType?.ToString());
    //        total += GetPrice(priceEntries, nameof(FloorType), config.FloorType?.ToString());

    //        if (config.OtherRooms.HasValue)
    //            total += GetPrice(priceEntries, nameof(OtherRoomType), config.OtherRooms.Value.ToString());

    //        if (config.TerraceType.HasValue)
    //            total += GetPrice(priceEntries, nameof(TerraceType), config.TerraceType.Value.ToString());

    //        if (config.WinterGardenType.HasValue)
    //            total += GetPrice(priceEntries, nameof(WinterGardenType), config.WinterGardenType.Value.ToString());

    //        if (config.WithGuarantee)
    //            total += GetPrice(priceEntries, "Guarantee", "WithGuarantee");

    //        return total;
    //    }

    //    private decimal GetPrice(List<PriceConfigurationEntry> entries, string category, string key)
    //    {
    //        if (string.IsNullOrEmpty(key)) return 0;

    //        var price = entries
    //            .FirstOrDefault(e => e.Category == category && e.OptionKey == key);

    //        return price?.Price ?? 0;
    //    }
    //}

}
