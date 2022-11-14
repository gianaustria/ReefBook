using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Reefbook.Models
{
    public class ImagesModel
    {
        [Key]
        [Display(Name = "Friend ID")]
        public int imageID { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(30, ErrorMessage = "Invalid char count")]
        public int postID { get; set; }

        [Display(Name = "First Name")]
        [MaxLength(30, ErrorMessage = "Invalid char count")]
        public int profileID { get; set; }

        [Display(Name = "Profile ID")]
        [MaxLength(10, ErrorMessage = "Invalid char count")]
        public string image { get; set; }
    }
}