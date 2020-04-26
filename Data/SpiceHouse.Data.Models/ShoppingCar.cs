using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SpiceHouse.Data.Models
{
    public class ShoppingCar
    {
        public ShoppingCar()
        {
            this.ItemsCount = 1;
        }


        public int Id { get; set; }

        public string ApplicationUserId { get; set; }

        [NotMapped]
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public int MenuItemId { get; set; }

        [NotMapped]
        [ForeignKey("MenuItemId")]
        public virtual MenuItem MenuItem { get; set; }


        [Range(1, int.MaxValue, ErrorMessage = "The value should be greater or equal to {1}")]
        public int ItemsCount { get; set; }
    }
}
