using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SVS.Data;

namespace TestProject1.Utilities
{
    class Utilities
    {
        #region snippet1
        public static DbContextOptions<ApplicationDbContext> TestDbContextOptions()
        {
            // Create a new service provider to create a new in-memory database.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance using an in-memory database and 
            // IServiceProvider that the context should resolve all of its 
            // services from.
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("SVS")
                .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }
        #endregion
    }
}
