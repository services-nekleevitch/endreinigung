namespace App.Shared.Models.Admin
{
    public class PriceConfigurationModel
    {
        public Dictionary<RoomCount, double> RoomPrices { get; set; } = new();
        public Dictionary<BathroomCount, double> BathroomPrices { get; set; } = new();
        public Dictionary<PollutionLevel, double> PollutionPrices { get; set; } = new();
        public Dictionary<WindowType, double> WindowPrices { get; set; } = new();
        public Dictionary<FloorType, double> FloorPrices { get; set; } = new();
        public Dictionary<OtherRoomType, double> OtherRoomPrices { get; set; } = new();
        public Dictionary<TerraceType, double> TerracePrices { get; set; } = new();
        public Dictionary<WinterGardenType, double> WinterGardenPrices { get; set; } = new();
        public double GuaranteePrice { get; set; }
    }
}
