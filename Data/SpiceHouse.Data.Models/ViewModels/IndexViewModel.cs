namespace SpiceHouse.Data.Models.ViewModels
{
    using System.Collections.Generic;

    public class IndexViewModel
    {
        public IEnumerable<MenuItem> MenuItem { get; set; }

        public IEnumerable<Category> Category { get; set; }

        public IEnumerable<Coupon> Coupon { get; set; }
    }
}
