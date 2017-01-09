namespace WorldTour.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Booking")]
    public partial class Booking
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Booking()
        {
            Tickets = new HashSet<Tickets>();
        }

        public int BookingID { get; set; }

        public int FlightID { get; set; }

        [Column("Total Price")]
        public decimal Total_Price { get; set; }

        public int Quantity { get; set; }

        [Required]
        public string BookingName { get; set; }

        [Required]
        public string Email { get; set; }

        public string Phone { get; set; }

        public int? ReturnFlightID { get; set; }

        [Required]
        [StringLength(50)]
        public string Class { get; set; }

        public virtual Flights Flights { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tickets> Tickets { get; set; }
    }
}
