namespace SVS.Areas.Identity.Pages.Account.Manage.Storage
{
    using System.Threading.Tasks;
    using Areas.Storage.Data;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using Model;

    [Authorize(Roles = "Admin, Employee")]
    public class DeleteModel : PageModel
    {
        private readonly ParcelContext _context;

        public DeleteModel(ParcelContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Parcel Parcel { get; set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            this.Parcel = await _context.Parcels.FirstOrDefaultAsync(m => m.ID == id);

            if (this.Parcel != null)
            {
                _context.Parcels.Remove(this.Parcel);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}