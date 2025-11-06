using App.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppServices.Models
{
    public class PropertyConfigurationModel
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
    }
}
