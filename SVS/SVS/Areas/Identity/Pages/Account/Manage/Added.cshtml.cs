namespace SVS.Areas.Identity.Pages.Account.Manage
{
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class UserAddedModel : PageModel
    {
        public string RegisteredUser { get; set; }

        public void OnGetAsync(string User = null)
        {
            this.RegisteredUser = User;
        }
    }
}