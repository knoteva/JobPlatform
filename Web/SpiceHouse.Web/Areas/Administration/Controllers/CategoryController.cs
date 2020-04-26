using Microsoft.AspNetCore.Authorization;
using SpiceHouse.Common;

namespace SpiceHouse.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using SpiceHouse.Data;
    using SpiceHouse.Data.Models;

    [Area("Administration")]
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        // Create constructor for db
        public CategoryController(ApplicationDbContext db)
        {
            this._db = db;
        }

        [TempData]
        public string StatusMessage { get; set; }

        // GET: Pass all to the view.
        public async Task<IActionResult> Index()
        {
            var categories = await this._db.Categories.ToListAsync();
            return this.View(categories);
        }

        // GET: Create
        public IActionResult Create()
        {
            return this.View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (this.ModelState.IsValid)
            {
                var categoryExists = this._db.Categories.Any(c => c.Name == category.Name);
                if (categoryExists)
                {
                    this.ModelState.AddModelError("Name", "Error: The \"" + category.Name + "\" already exist");
                    return this.View(category);
                }
                else
                {
                    this._db.Categories.Add(category);
                    await this._db.SaveChangesAsync();

                    return this.RedirectToAction(nameof(this.Index));
                }
            }

            return this.View(category);
        }

        // GET: Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var category = await this._db.Categories.FindAsync(id);
            if (category == null)
            {
                return this.NotFound();
            }

            return this.View(category);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(category);
            }

            this._db.Update(category);
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

            var category = await this._db.Categories.FindAsync(id);
            if (category == null)
            {
                return this.NotFound();
            }

            return this.View(category);
        }

        // POST: Delete
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await this._db.Categories.FindAsync(id);

            if (category == null)
            {
                return this.View();
            }

            foreach (var sub in this._db.SubCategories)
            {
                if (sub.CategoryId == id)
                {
                    this._db.SubCategories.Remove(sub);
                }
            }

            foreach (var menuItem in this._db.MenuItems)
            {
                if (menuItem.CategoryId == id)
                {
                    this._db.MenuItems.Remove(menuItem);
                }
            }

            this._db.Categories.Remove(category);
            await this._db.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        // GET: Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var category = await this._db.Categories.FindAsync(id);
            if (category == null)
            {
                return this.NotFound();
            }

            return this.View(category);
        }
    }
}
