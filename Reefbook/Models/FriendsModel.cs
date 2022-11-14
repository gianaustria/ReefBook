using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Reefbook.Models
{
    public class FriendsModel
    {
        [Key]
        [Display(Name = "Friend ID")]
        public int friendID { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(ErrorMessage = "Invalid char count")]
        [Required(ErrorMessage = "Required")]
        public string friendLastname { get; set; }

        [Display(Name = "First Name")]
        [MaxLength(ErrorMessage = "Invalid char count")]
        [Required(ErrorMessage = "Required")]
        public string friendFirstname { get; set; }

        [Display(Name = "Profile ID")]
        [MaxLength(ErrorMessage = "Invalid char count")]
        [Required(ErrorMessage = "Required")]
        public int profileID { get; set; }

     
    }
}