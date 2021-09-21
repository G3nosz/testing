namespace SVS.Areas.Storage.Data
{
    using Microsoft.EntityFrameworkCore;
    using Model;

    public class ParcelContext : DbContext
    {
        public ParcelContext(DbContextOptions<ParcelContext> options)
            : base(options)
        {
        }

        public DbSet<Parcel> Parcels { get; set; }
    }
}