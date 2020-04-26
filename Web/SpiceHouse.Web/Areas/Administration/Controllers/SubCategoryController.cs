using Microsoft.AspNetCore.Authorization;
using SpiceHouse.Common;

namespace SpiceHouse.Web.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using SpiceHouse.Data;
    using SpiceHouse.Data.Models;
    using SpiceHouse.Data.Models.ViewModels;

    [Area("Administration")]
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public class SubCategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SubCategoryController(ApplicationDbContext db)
        {
            this._db = db;
        }

        [TempData]
        public string StatusMessage { get; set; }

        // Get INDEX
        public async Task<IActionResult> Index()
        {
            var subCategories = await this._db.SubCategories.Include(s => s.Category).ToListAsync();
            return this.View(subCategories);
        }

        // GET - CREATE
        public async Task<IActionResult> Create()
        {
            var categories = await this._db.Categories.ToListAsync();
            if (!categories.Any())
            {
                return this.Redirect("/Administration/Category");
            }

            var model = new SubCategoryCategoryViewModel
            {
                SubCategory = new SubCategory(),
                SubCategoryList = await this._db.SubCategories.OrderBy(p => p.Name).Select(p => p.Name).ToListAsync(),
                CategoryList = await this._db.Categories.ToListAsync(),
            };

            return this.View(model);
        }

        // POST: CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubCategoryCategoryViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var subCategoryExists = this._db.SubCategories.Include(s => s.Category).Where(s => s.Name == model.SubCategory.Name && s.Category.Id == model.SubCategory.CategoryId);

                if (subCategoryExists.Any())
                {
                    this.StatusMessage = "Error: The " + subCategoryExists.First().Name + " already exist";
                }
                else
                {
                    this._db.SubCategories.Add(model.SubCategory);
                    await this._db.SaveChangesAsync();

                    // var category = this._db.Categories.FirstOrDefault(c => c.Id == model.SubCategory.CategoryId);
                    // category.SubCategories.Add(model.SubCategory);
                    return this.RedirectToAction(nameof(this.Index));
                }
            }

            var modelVm = new SubCategoryCategoryViewModel()
            {
                CategoryList = await this._db.Categories.ToListAsync(),
                SubCategory = model.SubCategory,
                SubCategoryList = await this._db.SubCategories.OrderBy(p => p.Name).Select(p => p.Name).ToListAsync(),
                StatusMessage = this.StatusMessage,
            };


            return this.View(modelVm);
        }

        [ActionName("GetSubCategory")]
        public async Task<IActionResult> GetSubCategory(int id)
        {
            var subCategories = new List<SubCategory>();

            subCategories = await (from subCategory in this._db.SubCategories
                                   where subCategory.CategoryId == id
                                   select subCategory).ToListAsync();
            return this.Json(new SelectList(subCategories, "Id", "Name"));
        }

        // GET: EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            var subCategory = await this._db.SubCategories.SingleOrDefaultAsync(m => m.Id == id);

            if (subCategory == null || id == null)
            {
                return this.NotFound();
            }

            var model = new SubCategoryCategoryViewModel()
            {
                CategoryList = await this._db.Categories.ToListAsync(),
                SubCategory = subCategory,
                SubCategoryList = await this._db.SubCategories.OrderBy(p => p.Name).Select(p => p.Name).Distinct().ToListAsync(),
            };

            return this.View(model);
        }

        // POST: EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SubCategoryCategoryViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var doesSubCategoryExists = this._db.SubCategories.Include(s => s.Category).Where(s => s.Name == model.SubCategory.Name && s.Category.Id == model.SubCategory.CategoryId);

                if (doesSubCategoryExists.Any())
                {
                    this.StatusMessage = "Error: The " + doesSubCategoryExists.First().Name + " already exist in " + doesSubCategoryExists.First().Category.Name;
                }
                else
                {
                    var subCatFromDb = await this._db.SubCategories.FindAsync(model.SubCategory.Id);
                    subCatFromDb.Name = model.SubCategory.Name;

                    await this._db.SaveChangesAsync();
                    return this.RedirectToAction(nameof(this.Index));
                }
            }

            var modelVM = new SubCategoryCategoryViewModel()
            {
                CategoryList = await this._db.Categories.ToListAsync(),
                SubCategory = model.SubCategory,
                SubCategoryList = await this._db.SubCategories.OrderBy(p => p.Name).Select(p => p.Name).ToListAsync(),
                StatusMessage = this.StatusMessage,
            };

            return this.View(modelVM);
        }

        // GET: Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var subCategory = await this._db.SubCategories.Include(s => s.Category).SingleOrDefaultAsync(m => m.Id == id);
            if (subCategory == null)
            {
                return this.NotFound();
            }

            return this.View(subCategory);
        }

        // POST: Delete
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subCategory = await this._db.SubCategories.SingleOrDefaultAsync(m => m.Id == id);
            this._db.SubCategories.Remove(subCategory);
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

            var subCategory = await this._db.SubCategories.Include(s => s.Category).SingleOrDefaultAsync(c => c.Id == id);
            if (subCategory == null)
            {
                return this.NotFound();
            }

            return this.View(subCategory);
        }
    }
}
