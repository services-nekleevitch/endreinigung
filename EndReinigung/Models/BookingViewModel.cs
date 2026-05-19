using System.ComponentModel.DataAnnotations;

namespace EndReinigung.Models
{
    public class BookingViewModel
    {
        // Discriminator: "endreinigung" (default) or "baureinigung". Selects the pricing table
        // and the extras layout used in confirmation emails.
        [StringLength(20)]
        public string Service { get; set; } = "endreinigung";

        [Required, StringLength(20)]
        public string PropertyType { get; set; } = "apartment";

        [Required, StringLength(10)]
        public string RoomSize { get; set; } = "";

        [Required, StringLength(4), MinLength(4)]
        public string ZipCode { get; set; } = "";

        [Range(0, 10)] public int Balcony { get; set; }
        [Range(0, 10)] public int UtilityBalcony { get; set; }
        [Range(0, 10)] public int Bath { get; set; }
        [Range(0, 10)] public int Wc { get; set; }
        [Range(0, 20)] public int Carpet { get; set; }
        [Range(0, 10)] public int BalconyPressure { get; set; }
        [Range(0, 10)] public int GaragePressure { get; set; }

        public bool Basement { get; set; }

        // Baureinigung-only toggle add-ons.
        public bool Bauschutt { get; set; }
        public bool Fassade { get; set; }

        [DataType(DataType.Date)]
        public DateTime? CleaningDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? HandoverDate { get; set; }

        [StringLength(10)]
        public string? HandoverTime { get; set; }

        public bool HandoverDateNotFixed { get; set; }

        [Required, StringLength(100)] public string FirstName { get; set; } = "";
        [Required, StringLength(100)] public string LastName { get; set; } = "";
        [Required, EmailAddress, StringLength(200)] public string Email { get; set; } = "";
        [Required, Phone, StringLength(50)] public string Phone { get; set; } = "";

        [Required, StringLength(200)] public string CustomerStreet { get; set; } = "";
        [Required, StringLength(4), MinLength(4)] public string CustomerPlz { get; set; } = "";
        [Required, StringLength(100)] public string CustomerCity { get; set; } = "";

        public bool SameAsCustomer { get; set; }

        [StringLength(200)] public string? ObjectStreet { get; set; }
        [StringLength(4)] public string? ObjectPlz { get; set; }
        [StringLength(100)] public string? ObjectCity { get; set; }

        [StringLength(2000)] public string? Notes { get; set; }

        [Required, StringLength(20)] public string PaymentMethod { get; set; } = "";

        public decimal QuotedTotal { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "AGB müssen akzeptiert werden.")]
        public bool TermsAccepted { get; set; }
    }
}
