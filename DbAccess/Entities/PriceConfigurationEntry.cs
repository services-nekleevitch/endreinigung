using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbAccess.Entities
{
    public class PriceConfigurationEntry
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PriceConfigurationSetId { get; set; }

        [ForeignKey("PriceConfigurationSetId")]
        public PriceConfigurationSet ConfigurationSet { get; set; }

        [Required]
        public string Category { get; set; } // e.g., "RoomCount"

        [Required]
        public string OptionKey { get; set; } // e.g., "Three"

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
