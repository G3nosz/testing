using Microsoft.AspNetCore.Hosting;
using SVS.Areas.Identity;

[assembly: HostingStartup(typeof(IdentityHostingStartup))]

namespace SVS.Areas.Identity
{
    using Data;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using SVS.Data;

    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<SVSContext>(options =>
                                                      options.UseSqlServer(
                                                          context.Configuration.GetConnectionString("SVSContextConnection")));

                services.AddDefaultIdentity<SVSUser>(options =>
                         {
                             options.SignIn.RequireConfirmedAccount     = false;
                             options.SignIn.RequireConfirmedEmail       = false;
                             options.SignIn.RequireConfirmedPhoneNumber = false;
                         }).
                         AddRoles<IdentityRole>().
                         AddEntityFrameworkStores<SVSContext>();
            });
        }
    }
}