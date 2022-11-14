using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Reefbook.Models
{
    public class ProductsModel
    {

        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Required.")]
        [MaxLength(100, ErrorMessage = "Invalid char. length")]
        public string Name { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = "Select from a list.")]
        public int CatID { get; set; }
        public List<CategoriesModel> Categories { get; set; }
        public string Category { get; set; }

        [Required(ErrorMessage = "Required.")]
        [MaxLength(20, ErrorMessage = "Invalid char. length")]
        public string Code { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Required.")]
        [DataType(DataType.Upload)]
        public string Image { get; set; }

        [Required(ErrorMessage = "Required.")]
        [Range(10.00, 100000.00, ErrorMessage = "Invalid amount.")]
        public decimal Price { get; set; }

        [Display(Name = "Featured Product?")]
        public bool IsFeatured { get; set; }

        public int Available { get; set; }

        [Display(Name = "Critical Level")]
        [Range(0, 100, ErrorMessage = "Invalid value.")]
        public int Critical { get; set; }

        [Range(0, 100, ErrorMessage = "Invalid value.")]
        public int Maximum { get; set; }

        public string Status { get; set; }

        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; }

        [Display(Name = "Date Modified")]
        public DateTime? DateModified { get; set; }
    }
    public class CategoriesModel
    {
        public int CatID { get; set; }
        public string Category { get; set; }
        public int TotalCount { get; set; }
    }
}