using Microsoft.AspNetCore.Hosting;
using SVS.Areas.Storage;

[assembly: HostingStartup(typeof(StorageHostingStartup))]

namespace SVS.Areas.Storage
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using SVS.Data;

    public class StorageHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<SVSContext>(options =>
                                                      options.UseSqlServer(
                                                          context.Configuration.GetConnectionString("SVSContextConnection")));
            });
        }
    }
}