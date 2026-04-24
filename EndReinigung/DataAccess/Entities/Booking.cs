using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EndReinigung.DataAccess.Entities
{
    public class Booking
    {
        [Key]
        public long Id { get; set; }

        /// <summary>Human-readable identifier, e.g. ZR-20260424-A7F2C9. Unique.</summary>
        [Required, MaxLength(32)]
        public string BookingNumber { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // ===== Property =====
        [Required, MaxLength(20)] public string PropertyType { get; set; } = "";
        [Required, MaxLength(10)] public string RoomSize { get; set; } = "";
        [Required, MaxLength(4)]  public string ZipCode { get; set; } = "";

        // ===== Extras =====
        public int Balcony { get; set; }
        public int UtilityBalcony { get; set; }
        public int Bath { get; set; }
        public int Wc { get; set; }
        public int Carpet { get; set; }
        public int BalconyPressure { get; set; }
        public int GaragePressure { get; set; }
        public bool Basement { get; set; }

        // ===== Dates =====
        public DateTime? CleaningDate { get; set; }
        public DateTime? HandoverDate { get; set; }
        [MaxLength(10)] public string? HandoverTime { get; set; }
        public bool HandoverDateNotFixed { get; set; }

        // ===== Contact =====
        [Required, MaxLength(100)] public string FirstName { get; set; } = "";
        [Required, MaxLength(100)] public string LastName { get; set; } = "";
        [Required, MaxLength(200)] public string Email { get; set; } = "";
        [Required, MaxLength(50)]  public string Phone { get; set; } = "";

        // ===== Addresses =====
        [Required, MaxLength(200)] public string CustomerStreet { get; set; } = "";
        [Required, MaxLength(4)]   public string CustomerPlz { get; set; } = "";
        [Required, MaxLength(100)] public string CustomerCity { get; set; } = "";

        public bool SameAsCustomer { get; set; }

        [MaxLength(200)] public string? ObjectStreet { get; set; }
        [MaxLength(4)]   public string? ObjectPlz { get; set; }
        [MaxLength(100)] public string? ObjectCity { get; set; }

        [MaxLength(2000)] public string? Notes { get; set; }

        [Required, MaxLength(20)] public string PaymentMethod { get; set; } = "";

        // ===== Money =====
        /// <summary>Authoritative price computed server-side at submission time.</summary>
        [Column(TypeName = "decimal(10,2)")]
        public decimal ServerPrice { get; set; }

        /// <summary>Price the client displayed (from JS); kept for reconciliation if it differs.</summary>
        [Column(TypeName = "decimal(10,2)")]
        public decimal? ClientQuotedPrice { get; set; }

        // ===== Status tracking =====
        public BookingStatus Status { get; set; } = BookingStatus.New;

        /// <summary>Internal notification email to info@ (ops inbox).</summary>
        public bool EmailSent { get; set; }
        public DateTime? EmailSentAt { get; set; }
        [MaxLength(1000)] public string? EmailError { get; set; }

        /// <summary>Confirmation email to the customer's email address.</summary>
        public bool CustomerEmailSent { get; set; }
        public DateTime? CustomerEmailSentAt { get; set; }
        [MaxLength(1000)] public string? CustomerEmailError { get; set; }
    }

    public enum BookingStatus
    {
        New = 0,
        EmailSent = 1,
        Confirmed = 2,
        Cancelled = 3,
        Completed = 4,
    }
}
