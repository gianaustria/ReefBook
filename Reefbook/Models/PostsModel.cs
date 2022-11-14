using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Reefbook.Models
{
    public class PostsModel
    {
        [Key]
        [Display(Name = "Post ID")]
        public int postID { get; set; }

        public int profileID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Display(Name = "Post")]
        [MaxLength(100, ErrorMessage = "Invalid char count")]
        [DataType(DataType.MultilineText)]
        public string post { get; set; }

        public string image { get; set; }

        [Display(Name = "Event Title")]
        [Required(ErrorMessage = "Required")]

        public string eventTitle { get; set; }
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Required")]
        public string eventDesc { get; set; }
        [Display(Name = "Date")]
        [Required(ErrorMessage = "Required")]
        public DateTime eventDate { get; set; }
        [DataType(DataType.Upload)]
        [Required(ErrorMessage = "Required")]
        public string eventImage { get; set; }

    }

    //public class EventsModel
    //{
    //    [Key]
    //    public int eventID { get; set; }
    //    [Display(Name = "Event Title")]
    //    [Required(ErrorMessage = "Required")]

    //    public string eventTitle { get; set; }
    //    [Display(Name = "Description")]
    //    [DataType(DataType.MultilineText)]
    //    [Required(ErrorMessage = "Required")]
    //    public string eventDesc { get; set; }
    //    [Display(Name = "Date")]
    //    [Required(ErrorMessage = "Required")]
    //    public DateTime eventDate { get; set; }
    //    [DataType(DataType.Upload)]
    //    [Required(ErrorMessage = "Required")]
    //    public string image { get; set; }
    //}


}