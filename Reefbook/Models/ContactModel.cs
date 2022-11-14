using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Reefbook.Models
{
    public class ContactModel
    {
        public string Name { get; set; }
        [DataType(DataType.EmailAddress), Display(Name = "To")]
        [Required]
        public string Email { get; set; }
        [DataType(DataType.MultilineText)]
        public string Feedback { get; set; }
    }
}