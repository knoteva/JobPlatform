namespace SpiceHouse.Data.Models.ViewModels
{
    using System.Collections.Generic;

    public class SubCategoryCategoryViewModel
    {
        public IEnumerable<Category> CategoryList { get; set; }

        public SubCategory SubCategory { get; set; }

        public List<string> SubCategoryList { get; set; }

        public string StatusMessage { get; set; }
    }
}
