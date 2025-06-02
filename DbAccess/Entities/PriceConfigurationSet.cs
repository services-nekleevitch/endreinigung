using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbAccess.Entities
{
    public class PriceConfigurationSet
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string VersionName { get; set; } // e.g., "Initial", "Spring2025"

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<PriceConfigurationEntry> Entries { get; set; }
    }
}
