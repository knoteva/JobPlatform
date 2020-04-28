namespace SpiceHouse.Web.ViewComponents
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using SpiceHouse.Data;

    public class UserNameViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public UserNameViewComponent(ApplicationDbContext db)
        {
            this._db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var userFromDb = await this._db.Users.FirstOrDefaultAsync(u => u.Id == claims.Value);

            return this.View(userFromDb);
        }

    }
}
