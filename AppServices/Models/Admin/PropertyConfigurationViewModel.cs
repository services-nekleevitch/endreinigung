using System.ComponentModel.DataAnnotations;
using App.Shared;

namespace EndreinigungZurich.Models.Booking
{
    public class PropertyConfigurationViewModel
    {
        [Required(
ErrorMessageResourceType = typeof(AppServices.Resources.SharedResources),
ErrorMessageResourceName = "Rooms_Required")]
        public RoomCount? Rooms { get; set; }
        [Required(
ErrorMessageResourceType = typeof(AppServices.Resources.SharedResources),
ErrorMessageResourceName = "Bathrooms_Required")]
        public BathroomCount? Bathrooms { get; set; }
        [Required(
    ErrorMessageResourceType = typeof(AppServices.Resources.SharedResources),
    ErrorMessageResourceName = "PollutionLevel_Required")]
        public PollutionLevel? PollutionLevel { get; set; }

        [Required(
ErrorMessageResourceType = typeof(AppServices.Resources.SharedResources),
ErrorMessageResourceName = "WindowType_Required")]
        public WindowType? WindowType { get; set; }

        [Required(
ErrorMessageResourceType = typeof(AppServices.Resources.SharedResources),
ErrorMessageResourceName = "FloorType_Required")]
        public FloorType? FloorType { get; set; }
        public OtherRoomType? OtherRooms { get; set; }
        public TerraceType? TerraceType { get; set; }
        public WinterGardenType? WinterGardenType { get; set; }
        public bool WithGuarantee { get; set; }
        public string AdditionalInfo { get; set; }

        public double CalculatedTotalCost { get; set; }
        public double TotalCost { get; internal set; }
    }
}
