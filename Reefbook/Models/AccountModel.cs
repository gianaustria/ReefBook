using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Reefbook.Models
{
    public class AccountModel
    {
        [Key]
        [Display(Name = "Account ID")]
        public int acccountID { get; set; }

        [Display(Name = "Account Number")]
        [MaxLength(20, ErrorMessage = "Invalid char count")]
        [Required(ErrorMessage = "Required")]
        public int accountNumber { get; set; }

        [Display(Name = "Account Pin")]
        [MaxLength(20, ErrorMessage = "Invalid char count")]
        [Required(ErrorMessage = "Required")]
        public int accountPIN { get; set; }
    }
}