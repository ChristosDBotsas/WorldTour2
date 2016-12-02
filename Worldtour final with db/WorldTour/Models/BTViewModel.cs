using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorldTour.Models
{
    public class BTViewModel
    {
        //Data from Tickets
        [Required]
        public List<Tickets> Tickets { get; set; }

        //Data from Booking
        public int FID { get; set; }

        [Required]
        [StringLength(50)]
        public string Class { get; set; }

        public int? RFID { get; set; }

        [Column("Total Price")]
        public decimal Total_Price { get; set; }
        public int Quantity { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 2)]
        [RegularExpression(@"^[a-zA-Z\s]*$")]
        public string BookingName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [RegularExpression(@"^[0-9]{7,25}$")]
        public string Phone { get; set; }
        public int Children { get; set; }
    }
}