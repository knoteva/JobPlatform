namespace SpiceHouse.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using SpiceHouse.Data.Models.Enums;

    public class MenuItem
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Spicyness { get; set; }

        public ESpicy ESpicy { get; set; }

        public string Picture { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = " Please enter price greater than {1} lev!")]
        public double Price { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [Display(Name = "SubCategory")]
        public int SubCategoryId { get; set; }

        [ForeignKey("SubCategoryId")]
        public virtual SubCategory SubCategory { get; set; }
    }
}