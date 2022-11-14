using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Reefbook.Models
{
    public class EventsModel
    {
        [Key]
        [Display(Name = "Event ID")]
        public int ID { get; set; }

        [Display(Name = "Even Title")]
        [MaxLength(30, ErrorMessage = "Invalid char count")]
        public string eventTitle { get; set; }

        [Display(Name = "Event Description")]
        [MaxLength(1000, ErrorMessage = "Invalid char count")]
        [Required(ErrorMessage = "Required")]
        public string eventDescription { get; set; }

        [Display(Name = "Event Date")]
        public DateTime eventDate { get; set; }

        [Display(Name = "Profile ID")]
        [MaxLength(10, ErrorMessage = "Invalid char count")]
        public int profileID { get; set; }

        [Display(Name = "Image")]
        [MaxLength(10, ErrorMessage = "Invalid char count")]
        public string image { get; set; }
    }
}