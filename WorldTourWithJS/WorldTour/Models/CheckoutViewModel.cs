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
        [StringLength(60, MinimumLength = 6, ErrorMessage = "Name must be at least 6 characters long.")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Name can only have English letters and spaces.")]
        public string CardHolder { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{13,19}$", ErrorMessage = "Card number must have a minimum of 13 numbers and a maximum of 19 numbers, spaces are not allowed.")]
        public string CardNumber { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{3,4}$", ErrorMessage = "CVV consists of 3 or 4 numbers.")]
        public int CVV { get; set; }

        public string errmessage { get; set; }

    }
}