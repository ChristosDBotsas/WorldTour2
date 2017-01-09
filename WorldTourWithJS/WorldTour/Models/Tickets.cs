namespace WorldTour.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tickets
    {
        [Key]
        public int TicketID { get; set; }

        public int BookingID { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "Name must be at least 2 characters long.")]
        [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Name can only have English letters.")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "Name must be at least 2 characters long.")]
        [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Name can only have English letters.")]
        public string LastName { get; set; }

        public virtual Booking Booking { get; set; }
    }
}
