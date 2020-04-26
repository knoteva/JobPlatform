namespace SpiceHouse.Web.Areas.Customer.Controllers
{
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using SpiceHouse.Data;
    using SpiceHouse.Data.Models.ViewModels;
    using SpiceHouse.Web.Controllers;
    using SpiceHouse.Web.ViewModels;

    public class HomeController : BaseController
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            this._db = db;
        }

        public async Task<IActionResult> Index()
        {
            var indexViewModel = new IndexViewModel
            {
                MenuItem = await this._db.MenuItems.Include(m => m.Category).Include(m => m.SubCategory).ToListAsync(),
                Category = await this._db.Categories.ToListAsync(),
                Coupon = await this._db.Coupons.Where(c => c.IsActive == true).ToListAsync(),
            };

            return this.View(indexViewModel);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
