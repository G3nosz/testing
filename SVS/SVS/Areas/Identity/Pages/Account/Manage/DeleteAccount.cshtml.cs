namespace SVS.Areas.Identity.Pages.Account.Manage
{
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    [Authorize(Roles = "Admin")]
    public class AccountDelete : PageModel
    {
        private readonly SignInManager<SVSUser> _signInManager;
        private readonly UserManager<SVSUser>   userManager;

        public AccountDelete(
            UserManager<SVSUser>   userManager,
            SignInManager<SVSUser> signInManager)
        {
            this.userManager = userManager;
            _signInManager   = signInManager;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null || userManager.GetRolesAsync(user).Result.First() == "Admin")
            {
                return RedirectToPage("./Accounts");
            }

            var result = await userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return RedirectToPage("./Accounts");
            }

            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError("", error.Description);
            }

            return Page();
        }
    }
}