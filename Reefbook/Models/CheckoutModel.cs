using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Reefbook.Models
{
    public class CheckoutModel
    {
        public string OrderNo { get; set; }
        public string DateOrdered { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }

        [Required(ErrorMessage = "Required.")]
        [Display(Name = "First Name")]
        public string FN { get; set; }

        [Required(ErrorMessage = "Required.")]
        [Display(Name = "Last Name")]
        public string LN { get; set; }

        [Required(ErrorMessage = "Required.")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Required.")]
        public string Municipality { get; set; }

        [Required(ErrorMessage = "Required.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Required.")]
        public string Phone { get; set; }

        public string Quantity { get; set; }

        public string Amount { get; set; }

        public string Email { get; set; }
    }
}