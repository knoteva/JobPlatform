using System;
using System.Collections.Generic;
using System.Text;

namespace SpiceHouse.Data.Models.ViewModels
{
    public class OrderDetailsCarViewModel
    {
        public OrderDetailsCarViewModel()
        {
            this.ListProducts = new List<ShoppingCar>();

        }

        public List<ShoppingCar> ListProducts { get; set; }

        public Order Order { get; set; }
    }
}
