namespace SVS.Areas.Identity.Pages.Account.Manage.Storage
{
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class ParcelAddedModel : PageModel
    {
        public string RegisteredID { get; set; }

        public void OnGetAsync(string ID = null)
        {
            this.RegisteredID = ID;
        }
    }
}