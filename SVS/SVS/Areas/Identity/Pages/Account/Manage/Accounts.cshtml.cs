namespace SVS.Areas.Identity.Pages.Account.Manage
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    [Authorize(Roles = "Admin")]
    public class AccountsModel : PageModel
    {
        private readonly SignInManager<SVSUser> _signInManager;

        private readonly UserManager<SVSUser> _userManager;

        public AccountsModel(
            UserManager<SVSUser>   userManager,
            SignInManager<SVSUser> signInManager)
        {
            _userManager   = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]

        public IList<SVSUser> Users { get; set; }

        public string GetRole(SVSUser user)
        {
            return _userManager.GetRolesAsync(user).Result.First();
        }

        public string GetName(SVSUser user)
        {
            return _userManager.GetClaimsAsync(user).
                                Result.Where(x => x.Type == ClaimTypes.GivenName).
                                Select(x => x.Value).
                                Single() +
                   " "                   +
                   _userManager.GetClaimsAsync(user).
                                Result.Where(x => x.Type == ClaimTypes.Surname).
                                Select(x => x.Value).
                                Single();
        }


        public async Task OnGetAsync()
        {
            this.Users = await _userManager.Users.ToListAsync();
        }

        public IList<SVSUser> GetData()
        {
            return this.Users;
        }
    }
}