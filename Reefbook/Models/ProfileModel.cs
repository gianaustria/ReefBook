using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Reefbook.Models
{
    public class ProfileModel
    {
        [Key]
        public int profileID { get; set; }


        public int TypeID { get; set; }

        public List<TypesModel> UserTypes { get; set; }
        public string UserType { get; set; }

        [Display(Name = "Username")]

        [DataType(DataType.EmailAddress)]
        public string username { get; set; }

        [Display(Name = "Password")]

        [DataType(DataType.Password)]
        public string password { get; set; }

        [DataType(DataType.Upload)]
        [Required(ErrorMessage = "Required")]
        public string Image { get; set; }

        [Display(Name = "Lastname")]
        [MaxLength(50, ErrorMessage = "Invalid char count")]
        [Required(ErrorMessage = "Required")]
        public string profileLastname { get; set; }

        [Display(Name = "Firstname")]
        [MaxLength(50, ErrorMessage = "Invalid char count")]
        [Required(ErrorMessage = "Required")]
        public string profileFirstname { get; set; }

        [Display(Name = "ContactNo")]
        [MaxLength(12, ErrorMessage = "Invalid char count")]
        [Required(ErrorMessage = "Required")]
        public string profileContactNo { get; set; }

        [Display(Name = "Rank")]
        [MaxLength(20, ErrorMessage = "Invalid char count")]
        [Required(ErrorMessage = "Required")]
        public string profileRank { get; set; }

        [Display(Name = "Street")]
        [MaxLength(50, ErrorMessage = "Invalid char count")]
        [Required(ErrorMessage = "Required")]
        public string Street { get; set; }

        [Display(Name = "Municipality")]
        [MaxLength(50, ErrorMessage = "Invalid char count")]
        [Required(ErrorMessage = "Required")]
        public string Municipality { get; set; }

        [Display(Name = "City")]
        [MaxLength(50, ErrorMessage = "Invalid char count")]
        [Required(ErrorMessage = "Required")]
        public string City { get; set; }

        [Display(Name = "Recent Location")]
        [MaxLength(50, ErrorMessage = "Invalid char count")]
        [Required(ErrorMessage = "Required")]
        public string profileRecentLocation { get; set; }




    }

    public class TypesModel
    {
        public int TypeID { get; set; }
        public string UserType { get; set; }
    }
}