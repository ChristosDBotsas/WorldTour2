namespace WorldTour.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Flights
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Flights()
        {
            Booking = new HashSet<Booking>();
        }

        [Key]
        public int FlightID { get; set; }

        [Column("Company Name")]
        [Required]
        [StringLength(50)]
        public string Company_Name { get; set; }

        [Column("Departure Airport")]
        [Required]
        [StringLength(50)]
        public string Departure_Airport { get; set; }

        [Column("Arrival Airport")]
        [Required]
        [StringLength(50)]
        public string Arrival_Airport { get; set; }

        [Column("Available Seats")]
        public int Available_Seats { get; set; }

        [Column("Departure Date")]
        public DateTime Departure_Date { get; set; }

        [Column("Arrival Date")]
        public DateTime Arrival_Date { get; set; }

        [Column("Starting Price", TypeName = "money")]
        public decimal Starting_Price { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Booking> Booking { get; set; }
    }
}
