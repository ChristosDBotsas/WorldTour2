using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WorldTour.Models
{
    public class CheckoutViewModel : BTViewModel
    {
        [Required]
        [StringLength(60, MinimumLength = 2)]
        [RegularExpression(@"^[a-zA-Z\s]*$")]
        public string CardHolder { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{13,19}$")]
        public string CardNumber { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{3,4}$")]
        public int CVV { get; set; }

        public string errmessage { get; set; }

    }
}