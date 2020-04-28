using Microsoft.AspNetCore.Authorization;

namespace SpiceHouse.Web.Areas.Administration.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using SpiceHouse.Common;
    using SpiceHouse.Data;
    using SpiceHouse.Data.Models;
    using SpiceHouse.Data.Models.ViewModels;

    [Area("Administration")]
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public class MenuItemController : Controller
    {
        private readonly ApplicationDbContext _db;

        // dependency injection
        private readonly IWebHostEnvironment _hostingEnvironment;

        [BindProperty]
        public MenuItemViewModel MenuItemViewModel { get; set; }

        public MenuItemController(ApplicationDbContext db, IWebHostEnvironment hostingEnvironment)
        {
            this._db = db;
            this._hostingEnvironment = hostingEnvironment;
            this.MenuItemViewModel = new MenuItemViewModel()
            {
                Category = this._db.Categories,
                MenuItem = new MenuItem(),
            };
        }

        public async Task<IActionResult> Index()
        {
            var menuItems = await this._db.MenuItems.Include(m => m.Category).Include(m => m.SubCategory).ToListAsync();
            return this.View(menuItems);
        }

        // GET: Create
        public async Task<IActionResult> Create()
        {
            var categories = await this._db.Categories.ToListAsync();
            if (!categories.Any())
            {
                return this.Redirect("/Administration/Category");
            }

            return this.View(this.MenuItemViewModel);
        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]

        // MenuItemViewModel MenuItemViewModel: not needed because of the BindProperty
        public async Task<IActionResult> CreatePost()
        {
            var subCategoryIdToInt = 0;

            if (this.Request.Form["SubCategoryId"].Any())
            {
                subCategoryIdToInt = Convert.ToInt32(this.Request.Form["SubCategoryId"].ToString());
            }

            if (subCategoryIdToInt == 0)
            {
                return this.Redirect("/Administration/SubCategory");
            }
            else
            {
                this.MenuItemViewModel.MenuItem.SubCategoryId = subCategoryIdToInt;
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(this.MenuItemViewModel);
            }

            this._db.MenuItems.Add(this.MenuItemViewModel.MenuItem);
            await this._db.SaveChangesAsync();

            var webRootPath = this._hostingEnvironment.WebRootPath;
            var files = this.HttpContext.Request.Form.Files;

            var menuItemFromDb = await this._db.MenuItems.FindAsync(this.MenuItemViewModel.MenuItem.Id);

            if (files.Count > 0)
            {
                var uploads = Path.Combine(webRootPath, "images");
                var extension = Path.GetExtension(files[0].FileName);

                await using (var filesStream = new FileStream(Path.Combine(uploads, this.MenuItemViewModel.MenuItem.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(filesStream);
                }

                menuItemFromDb.Picture = @"\images\" + this.MenuItemViewModel.MenuItem.Id + extension;
            }
            else
            {
                var uploads = Path.Combine(webRootPath, @"images\" + GlobalConstants.DefaultFoodImage);
                System.IO.File.Copy(uploads, webRootPath + @"\images\" + this.MenuItemViewModel.MenuItem.Id + ".png");
                menuItemFromDb.Picture = @"\images\" + this.MenuItemViewModel.MenuItem.Id + ".png";
            }

            await this._db.SaveChangesAsync();

            return this.RedirectToAction(nameof(this.Index));
        }

        // GET: EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            this.MenuItemViewModel.MenuItem = await this._db.MenuItems.Include(m => m.Category).Include(m => m.SubCategory).SingleOrDefaultAsync(m => m.Id == id);
            this.MenuItemViewModel.SubCategory = await this._db.SubCategories.Where(s => s.CategoryId == this.MenuItemViewModel.MenuItem.CategoryId).ToListAsync();

            if (this.MenuItemViewModel.MenuItem == null)
            {
                return this.NotFound();
            }

            return this.View(this.MenuItemViewModel);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var subCategoryIdToInt = 0;

            if (this.Request.Form["SubCategoryId"].Any())
            {
                subCategoryIdToInt = Convert.ToInt32(this.Request.Form["SubCategoryId"].ToString());
            }

            if (subCategoryIdToInt == 0)
            {
                this.MenuItemViewModel.MenuItem.SubCategoryId = null;
            }
            else
            {
                this.MenuItemViewModel.MenuItem.SubCategoryId = subCategoryIdToInt;
            }

            if (!this.ModelState.IsValid)
            {
                this.MenuItemViewModel.SubCategory = await this._db.SubCategories.Where(s => s.CategoryId == this.MenuItemViewModel.MenuItem.CategoryId).ToListAsync();
                return this.View(this.MenuItemViewModel);
            }

            var webRootPath = this._hostingEnvironment.WebRootPath;
            var files = this.HttpContext.Request.Form.Files;

            var menuItemFromDb = await this._db.MenuItems.FindAsync(this.MenuItemViewModel.MenuItem.Id);

            if (files.Count > 0)
            {
                var uploads = Path.Combine(webRootPath, "images");
                var extensionNew = Path.GetExtension(files[0].FileName);

                var imagePath = Path.Combine(webRootPath, menuItemFromDb.Picture.TrimStart('\\'));

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                await using (var filesStream = new FileStream(Path.Combine(uploads, this.MenuItemViewModel.MenuItem.Id + extensionNew), FileMode.Create))
                {
                    files[0].CopyTo(filesStream);
                }

                menuItemFromDb.Picture = @"\images\" + this.MenuItemViewModel.MenuItem.Id + extensionNew;
            }

            menuItemFromDb.Name = this.MenuItemViewModel.MenuItem.Name;
            menuItemFromDb.Description = this.MenuItemViewModel.MenuItem.Description;
            menuItemFromDb.Price = this.MenuItemViewModel.MenuItem.Price;
            menuItemFromDb.Spicyness = this.MenuItemViewModel.MenuItem.Spicyness;
            menuItemFromDb.CategoryId = this.MenuItemViewModel.MenuItem.CategoryId;
            menuItemFromDb.SubCategoryId = this.MenuItemViewModel.MenuItem.SubCategoryId;

            await this._db.SaveChangesAsync();

            return this.RedirectToAction(nameof(this.Index));
        }



        // GET: Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            this.MenuItemViewModel.MenuItem = await this._db.MenuItems.Include(m => m.Category).Include(m => m.SubCategory).SingleOrDefaultAsync(m => m.Id == id);

            if (this.MenuItemViewModel.MenuItem == null)
            {
                return this.NotFound();
            }

            return this.View(this.MenuItemViewModel);
        }

        // POST: Delete
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var webRootPath = this._hostingEnvironment.WebRootPath;
            var menuItem = await this._db.MenuItems.FindAsync(id);
            var shoppingItem = await this._db.ShoppingCars.FindAsync(id);

            if (menuItem != null)
            {
                var imagePath = Path.Combine(webRootPath, menuItem.Picture.TrimStart('\\'));

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                this._db.MenuItems.Remove(menuItem);
                if (shoppingItem != null)
                {
                    this._db.ShoppingCars.Remove(shoppingItem);
                }

                await this._db.SaveChangesAsync();

            }

            return this.RedirectToAction(nameof(this.Index));
        }

        // GET: Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            this.MenuItemViewModel.MenuItem = await this._db.MenuItems.Include(m => m.Category).Include(m => m.SubCategory).SingleOrDefaultAsync(m => m.Id == id);

            if (this.MenuItemViewModel.MenuItem == null)
            {
                return this.NotFound();
            }

            return this.View(this.MenuItemViewModel);
        }
    }
}
