﻿using Microsoft.AspNetCore.Authorization;
using SpiceHouse.Common;

namespace SpiceHouse.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using SpiceHouse.Data;
    using SpiceHouse.Data.Models;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    [Area("Administration")]
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public class CouponController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CouponController(ApplicationDbContext db)
        {
            this._db = db;
        }

        // Pass coupons to the View
        public async Task<IActionResult> Index()
        {
            return this.View(await this._db.Coupons.ToListAsync());
        }

        // GET: Create
        public IActionResult Create()
        {
            return this.View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Coupon coupons)
        {
            if (this.ModelState.IsValid)
            {
                var files = this.HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[] pic;

                    await using (var fileStreamOne = files[0].OpenReadStream())
                    {
                        await using var memoryStreamOne = new MemoryStream();
                        fileStreamOne.CopyTo(memoryStreamOne);
                        pic = memoryStreamOne.ToArray();
                    }

                    coupons.Picture = pic;
                }

                this._db.Coupons.Add(coupons);
                await this._db.SaveChangesAsync();
                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(coupons);
        }

        // GET: Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var coupon = await this._db.Coupons.SingleOrDefaultAsync(m => m.Id == id);
            if (coupon == null)
            {
                return this.NotFound();
            }

            return this.View(coupon);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Coupon coupons)
        {
            if (coupons.Id == 0)
            {
                return this.NotFound();
            }

            var couponFromDb = await this._db.Coupons.Where(c => c.Id == coupons.Id).FirstOrDefaultAsync();

            if (this.ModelState.IsValid)
            {
                var files = this.HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[] pic = null;
                    await using (var fileStreamOne = files[0].OpenReadStream())
                    {
                        await using var memoryStreamOne = new MemoryStream();
                        fileStreamOne.CopyTo(memoryStreamOne);
                        pic = memoryStreamOne.ToArray();
                    }

                    couponFromDb.Picture = pic;
                }

                couponFromDb.MinimumAmount = coupons.MinimumAmount;
                couponFromDb.Name = coupons.Name;
                couponFromDb.Discount = coupons.Discount;
                couponFromDb.CouponType = coupons.CouponType;
                couponFromDb.IsActive = coupons.IsActive;

                await this._db.SaveChangesAsync();
                return this.RedirectToAction(nameof(Index));
            }

            return this.View(coupons);
        }

        // GET: Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var coupon = await this._db.Coupons.SingleOrDefaultAsync(c => c.Id == id);
            if (coupon == null)
            {
                return this.NotFound();
            }

            return this.View(coupon);
        }

        // POST: Delete
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coupon = await this._db.Coupons.SingleOrDefaultAsync(c => c.Id == id);
            this._db.Coupons.Remove(coupon);
            await this._db.SaveChangesAsync();
            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var coupon = await this._db.Coupons
                .FirstOrDefaultAsync(c => c.Id == id);
            if (coupon == null)
            {
                return this.NotFound();
            }

            return this.View(coupon);
        }
    }
}
