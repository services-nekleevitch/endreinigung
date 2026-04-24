using System.ComponentModel.DataAnnotations;

namespace EndReinigung.Models
{
    public class ContactFormViewModel
    {
        [Required, StringLength(100)]
        public string FirstName { get; set; } = "";

        [Required, StringLength(100)]
        public string LastName { get; set; } = "";

        [Required, EmailAddress, StringLength(200)]
        public string Email { get; set; } = "";

        [Phone, StringLength(50)]
        public string? Phone { get; set; }

        [Required, StringLength(300)]
        public string Address { get; set; } = "";

        [StringLength(10)]
        public string? Rooms { get; set; }

        [DataType(DataType.Date)]
        public DateTime? MoveOut { get; set; }

        [Required, StringLength(50)]
        public string Objektart { get; set; } = "";

        [Required, StringLength(50)]
        public string Stockwerk { get; set; } = "";

        [Required, StringLength(50)]
        public string Moebelierung { get; set; } = "";

        [StringLength(2000)]
        public string? Message { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
