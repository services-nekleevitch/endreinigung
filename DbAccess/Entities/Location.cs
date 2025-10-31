using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbAccess.Entities
{
    public class LocationCH
    {
        public int Id { get; set; }

        public string ZipCode { get; set; } = null!;

        public string PlaceName { get; set; } = null!;

        public string MunicipalityName { get; set; }

        public int? MunicipalityNo { get; set; }

        public string Canton { get; set; }

        public string ExtraDigit { get; set; }
    }
}
