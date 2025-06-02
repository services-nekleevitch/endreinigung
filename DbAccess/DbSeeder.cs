using App.Shared;
using DbAccess.Entities;

namespace DbAccess
{
    public class DbSeeder
    {
        public static void SeedInitialPriceConfiguration(AppDbContext context)
        {
            if (context.PriceConfigurationSets.Any())
                return; // Already seeded

            var set = new PriceConfigurationSet
            {
                VersionName = "Initial",
                CreatedAt = DateTime.UtcNow
            };

            context.PriceConfigurationSets.Add(set);
            context.SaveChanges(); // Save first to generate ID

            var entries = new List<PriceConfigurationEntry>();

            // RoomCount
            foreach (RoomCount val in Enum.GetValues(typeof(RoomCount)))
            {
                entries.Add(new PriceConfigurationEntry
                {
                    PriceConfigurationSetId = set.Id,
                    Category = nameof(RoomCount),
                    OptionKey = val.ToString(),
                    Price = 100 + (int)val * 10
                });
            }

            // BathroomCount
            foreach (BathroomCount val in Enum.GetValues(typeof(BathroomCount)))
            {
                entries.Add(new PriceConfigurationEntry
                {
                    PriceConfigurationSetId = set.Id,
                    Category = nameof(BathroomCount),
                    OptionKey = val.ToString(),
                    Price = 50 + (int)val * 20
                });
            }

            // PollutionLevel
            foreach (PollutionLevel val in Enum.GetValues(typeof(PollutionLevel)))
            {
                entries.Add(new PriceConfigurationEntry
                {
                    PriceConfigurationSetId = set.Id,
                    Category = nameof(PollutionLevel),
                    OptionKey = val.ToString(),
                    Price = 30 + (int)val * 25
                });
            }

            // WindowType
            foreach (WindowType val in Enum.GetValues(typeof(WindowType)))
            {
                entries.Add(new PriceConfigurationEntry
                {
                    PriceConfigurationSetId = set.Id,
                    Category = nameof(WindowType),
                    OptionKey = val.ToString(),
                    Price = 40
                });
            }

            // FloorType
            foreach (FloorType val in Enum.GetValues(typeof(FloorType)))
            {
                entries.Add(new PriceConfigurationEntry
                {
                    PriceConfigurationSetId = set.Id,
                    Category = nameof(FloorType),
                    OptionKey = val.ToString(),
                    Price = 50
                });
            }

            // OtherRoomType
            foreach (OtherRoomType val in Enum.GetValues(typeof(OtherRoomType)))
            {
                entries.Add(new PriceConfigurationEntry
                {
                    PriceConfigurationSetId = set.Id,
                    Category = nameof(OtherRoomType),
                    OptionKey = val.ToString(),
                    Price = 25
                });
            }

            // TerraceType
            foreach (TerraceType val in Enum.GetValues(typeof(TerraceType)))
            {
                entries.Add(new PriceConfigurationEntry
                {
                    PriceConfigurationSetId = set.Id,
                    Category = nameof(TerraceType),
                    OptionKey = val.ToString(),
                    Price = 40
                });
            }

            // WinterGardenType
            foreach (WinterGardenType val in Enum.GetValues(typeof(WinterGardenType)))
            {
                entries.Add(new PriceConfigurationEntry
                {
                    PriceConfigurationSetId = set.Id,
                    Category = nameof(WinterGardenType),
                    OptionKey = val.ToString(),
                    Price = 40
                });
            }

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
}
