namespace SVS.Areas.Identity.Pages.Account.Manage.Storage
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Areas.Storage.Data;
    using Data;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using Model;

    public class DetailsModel : PageModel
    {
        private readonly ParcelContext        _context;
        private readonly UserManager<SVSUser> _userManager;

        public DetailsModel(ParcelContext context, UserManager<SVSUser> userManager)
        {
            _context     = context;
            _userManager = userManager;
        }

        public Parcel Parcel    { get; set; }
        public string IsFragile { get; set; }
        public string fullName  { get; set; }

        public async Task GetDriverAsync(string id)
        {
            if (id == null || string.IsNullOrEmpty(id) || _userManager.Users.Count() < 2)
            {
                this.fullName += "-";
                return;
            }

            var driver = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (driver == null)
            {
                this.fullName += "-";
                return;
            }

            var claims = await _userManager.GetClaimsAsync(driver);
            this.fullName += claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
            this.fullName += " " + claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            this.Parcel = await _context.Parcels.Include(x => x.Sender).
                                         Include(x => x.Receiver).
                                         FirstOrDefaultAsync(m => m.ID == id);
            this.IsFragile = this.Parcel.Fragile ? "Yes" : "No";
            await GetDriverAsync(this.Parcel.Driver);
            if (this.Parcel == null)
            {
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnPost(string ID)
        {
            return RedirectToPage("./Edit", new {id = ID});
        }
    }
}