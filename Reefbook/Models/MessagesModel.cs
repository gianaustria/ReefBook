using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Reefbook.Models
{
    public class MessagesModel
    {
        [Key]
        [Display(Name = "Message ID")]
        public int messageID { get; set; }

        [Display(Name = "Profile ID")]
        [MaxLength(10, ErrorMessage = "Invalid char count")]
        public int profileID { get; set; }

        [Display(Name = "Message")]
        [MaxLength(100, ErrorMessage = "Invalid char count")]
        public string message { get; set; }

    }
}