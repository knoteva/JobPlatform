namespace SpiceHouse.Web.Areas.Customer.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using SpiceHouse.Common;
    using SpiceHouse.Data;
    using SpiceHouse.Data.Models;
    using SpiceHouse.Data.Models.ViewModels;

    [Area("Customer")]
    public class CarController : Controller
    {
        [BindProperty]
        public OrderDetailsCarViewModel DetailsCar { get; set; }

        private readonly ApplicationDbContext _db;

        // Create constructor for db
        public CarController(ApplicationDbContext db)
        {
            this._db = db;
        }

        public async Task<IActionResult> Index()
        {
            this.DetailsCar = new OrderDetailsCarViewModel()
            {
                Order = new Order(),
            };

            this.DetailsCar.Order.OrderTotal = 0;

            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var cart = this._db.ShoppingCars.Where(c => c.ApplicationUserId == claim.Value);
            if (claim != null)
            {
                this.DetailsCar.ListProducts = cart.ToList();
            }

            foreach (var list in this.DetailsCar.ListProducts)
            {
                list.MenuItem = await this._db.MenuItems.FirstOrDefaultAsync(m => m.Id == list.MenuItemId);
                this.DetailsCar.Order.OrderTotal += list.MenuItem.Price * list.Count;
                list.MenuItem.Description = GlobalConstants.ConvertToRawHtml(list.MenuItem.Description);
                if (list.MenuItem.Description.Length > 100)
                {
                    list.MenuItem.Description = list.MenuItem.Description.Substring(0, 99) + "...";
                }
            }

            this.DetailsCar.Order.OrderTotalOriginal = this.DetailsCar.Order.OrderTotal;

            return this.View(this.DetailsCar);
        }
    }
}
