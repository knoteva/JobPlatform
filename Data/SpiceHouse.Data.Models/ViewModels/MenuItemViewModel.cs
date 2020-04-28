namespace SpiceHouse.Data.Models.ViewModels
{
    using System.Collections.Generic;

    public class MenuItemViewModel
    {
        public MenuItem MenuItem { get; set; }

        public IEnumerable<Category> Category { get; set; }

        public IEnumerable<SubCategory> SubCategory { get; set; }
    }
}
