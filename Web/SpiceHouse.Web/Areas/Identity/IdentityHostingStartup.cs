using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(SpiceHouse.Web.Areas.Identity.IdentityHostingStartup))]

namespace SpiceHouse.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}