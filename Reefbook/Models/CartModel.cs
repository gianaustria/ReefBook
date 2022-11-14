using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Reefbook.Models
{
    public class CartModel
    {
        public int RefNo { get; set; }
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
    }

    public class OrdersViewModel
    {
        public List<CartModel> AllCartItems { get; set; }
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

        public double Amount { get; set; }

        public string Email { get; set; }

    }
}