using System;
using System.Collections.Generic;
using System.Text;

namespace SpiceHouse.Data.Models.ViewModels
{
    public class SubCategoryCategoryViewModel
    {
        public IEnumerable<Category> CategoryList { get; set; }

        public SubCategory SubCategory { get; set; }

        public List<string> SubCategoryList { get; set; }

        public string StatusMessage { get; set; }
    }
}
