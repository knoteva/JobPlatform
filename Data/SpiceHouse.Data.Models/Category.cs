namespace SpiceHouse.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Category
    {
        public Category()
        {
            this.SubCategories = new List<SubCategory>();
        }

        public int Id { get; set; }

        [Required]
        [Display(Name = "Category Name")]
        public string Name { get; set; }

        public ICollection<SubCategory> SubCategories { get; set; }
    }
}
