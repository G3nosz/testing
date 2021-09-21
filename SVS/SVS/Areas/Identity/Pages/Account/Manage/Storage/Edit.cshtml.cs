namespace SVS.Areas.Identity.Pages.Account.Manage.Storage
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Areas.Storage.Data;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using Model;

    [Authorize(Roles = "Admin, Employee, Driver")]
    public class EditModel : PageModel
    {
        private readonly ParcelContext _context;

        public List<SelectListItem> Status = new List<SelectListItem>
        {
            new SelectListItem {Text = "At Warehouse", Value = "Warehouse"},
            new SelectListItem {Text = "Shipped", Value      = "Shipped"},
            new SelectListItem {Text = "Delivered", Value    = "Delivered"}
        };

        public EditModel(ParcelContext context)
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

            if (this.Parcel == null)
            {
                return NotFound();
            }

            return Page();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!this.ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(this.Parcel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParcelExists(this.Parcel.ID))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToPage("./Index");
        }

        private bool ParcelExists(string id)
        {
            return _context.Parcels.Any(e => e.ID == id);
        }
    }
}