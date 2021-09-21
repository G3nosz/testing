namespace SVS.Areas.Identity.Pages.Account.Manage.Storage
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Areas.Storage.Data;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using Model;

    public class StorageModel : PageModel
    {
        private readonly ParcelContext _context;

        public StorageModel(ParcelContext context)
        {
            _context = context;
        }

        public IList<Parcel> Parcel { get; set; }

        public string IsFragile(Parcel parcel)
        {
            return parcel.Fragile ? "Yes" : "No";
        }


        public async Task OnGetAsync()
        {
            this.Parcel = await _context.Parcels.ToListAsync();
            if (User.IsInRole("Driver"))
            {
                this.Parcel = await _context.
                                    Parcels.Where(x => x.Status == "Shipped" &&
                                                       x.Driver == User.FindFirst(ClaimTypes.NameIdentifier).Value).
                                    ToListAsync();
            }

            //if (User.IsInRole("Admin") || User.IsInRole("Employee"))
            //{
            //    this.Parcel = await _context.Parcels.Where(x => x.Status == "Warehouse" || x.Status == "Shipped").ToListAsync();
            //}
        }

        private class DriverInfo
        {
            public string ID    { get; set; }
            public int    Count { get; set; }
        }
    }
}