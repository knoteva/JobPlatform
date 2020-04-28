namespace SpiceHouse.Data.Models.ViewModels
{
    using System.Collections.Generic;

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
