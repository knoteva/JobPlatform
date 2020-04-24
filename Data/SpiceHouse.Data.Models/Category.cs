namespace SpiceHouse.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc;

    public class Category
    {
        public Category()
        {
            this.SubCategories = new List<SubCategory>();
        }

        public int Id { get; set; }

        [Required]
        [Display(Name = "Category Name")]
        [Remote("IsExist", "Place", ErrorMessage = "Category already exist!")]
        public string Name { get; set; }

        public ICollection<SubCategory> SubCategories { get; set; }
    }
}
