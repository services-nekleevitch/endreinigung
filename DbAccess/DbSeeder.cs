using App.Shared;
using DbAccess;
using DbAccess.Entities;

public static class DbSeeder
{
    public static void SeedInitialPriceConfiguration(AppDbContext context)
    {
        if (context.PriceConfigurationSets.Any())
            return;

        var set = new PriceConfigurationSet
        {
            VersionName = "Initial",
            CreatedAt = DateTime.UtcNow
        };

        context.PriceConfigurationSets.Add(set);
        context.SaveChanges(); // get set.Id

        var entries = new List<PriceConfigurationEntry>();

        void AddEnumPrices<T>(string category, Func<T, decimal> priceFunc) where T : Enum
        {
            foreach (var value in Enum.GetValues(typeof(T)).Cast<T>())
            {
                entries.Add(new PriceConfigurationEntry
                {
                    PriceConfigurationSetId = set.Id,
                    Category = category,
                    OptionKey = value.ToString(),
                    Price = priceFunc(value)
                });
            }
        }

        AddEnumPrices<RoomCount>("RoomCount", val => 100 + Convert.ToInt32(val) * 10);
        AddEnumPrices<BathroomCount>("BathroomCount", val => 50 + Convert.ToInt32(val) * 15);
        AddEnumPrices<PollutionLevel>("PollutionLevel", val => 30 + (int)(object)val * 25);
        AddEnumPrices<WindowType>("WindowType", val => 20);
        AddEnumPrices<FloorType>("FloorType", val => 30);
        AddEnumPrices<OtherRoomType>("OtherRoomType", val => 25);
        AddEnumPrices<TerraceType>("TerraceType", val => 40);
        AddEnumPrices<WinterGardenType>("WinterGardenType", val => 40);

        // Guarantee
        entries.Add(new PriceConfigurationEntry
        {
            PriceConfigurationSetId = set.Id,
            Category = "Guarantee",
            OptionKey = "WithGuarantee",
            Price = 80
        });

        context.PriceConfigurationEntries.AddRange(entries);
        context.SaveChanges();
    }
}
