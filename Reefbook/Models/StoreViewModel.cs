using Reefbook.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Reefbook.Models
{
    public class StoreViewModel
    {
        public List<ProductsModel> AllProducts { get; set; }
        public List<CategoriesModel> AllCategories { get; set; }

    }
}