namespace SpiceHouse.Web.Controllers
{
    using System.Diagnostics;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using SpiceHouse.Common;
    using SpiceHouse.Data;
    using SpiceHouse.Data.Models;
    using SpiceHouse.Data.Models.ViewModels;
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

            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                var allItems = this._db.ShoppingCars.Where(u => u.ApplicationUserId == claim.Value).ToList();
                var allItemsCount = allItems.Sum(count => count.Count);

                this.HttpContext.Session.SetInt32(GlobalConstants.SsCarItemsCount, allItemsCount);
            }

            return this.View(indexViewModel);
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var menuItemDb = await this._db.MenuItems.Include(m => m.Category).Include(m => m.SubCategory).Where(m => m.Id == id).FirstOrDefaultAsync();

            var car = new ShoppingCar
            {
                MenuItem = menuItemDb,
                MenuItemId = menuItemDb.Id,
            };

            return this.View(car);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(ShoppingCar carObject)
        {
            carObject.Id = 0;
            if (this.ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)this.User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                carObject.ApplicationUserId = claim.Value;

                var cartFromDb = await this._db.ShoppingCars.Where(c => c.ApplicationUserId == carObject.ApplicationUserId && c.MenuItemId == carObject.MenuItemId).FirstOrDefaultAsync();

                if (cartFromDb == null)
                {
                    await this._db.ShoppingCars.AddAsync(carObject);
                }
                else
                {
                    cartFromDb.Count = cartFromDb.Count + carObject.Count;
                }

                await this._db.SaveChangesAsync();

                var count = this._db.ShoppingCars.Where(c => c.ApplicationUserId == carObject.ApplicationUserId).ToList().Count();
                this.HttpContext.Session.SetInt32(GlobalConstants.SsCarItemsCount, count);

                return this.RedirectToAction("Index");
            }
            else
            {
                var menuItemFromDb = await this._db.MenuItems.Include(m => m.Category).Include(m => m.SubCategory).Where(m => m.Id == carObject.MenuItemId).FirstOrDefaultAsync();

                var cartObj = new ShoppingCar()
                {
                    MenuItem = menuItemFromDb,
                    MenuItemId = menuItemFromDb.Id,
                };
                return this.View(cartObj);
            }
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
