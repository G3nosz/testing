namespace SVS.Areas.Identity.Pages.Account.Manage
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.Mvc.Rendering;

    [Authorize(Roles = "Admin")]
    public class AccountsEdit : PageModel
    {
        private readonly SignInManager<SVSUser> _signInManager;
        private readonly UserManager<SVSUser>   userManager;

        public List<SelectListItem> Roles = new List<SelectListItem>
        {
            new SelectListItem {Text = "Employee", Value = "Employee"},
            new SelectListItem {Text = "Driver", Value   = "Driver"}
        };

        public AccountsEdit(
            UserManager<SVSUser>   userManager,
            SignInManager<SVSUser> signInManager)

        {
            this.userManager = userManager;
            _signInManager   = signInManager;
        }

        [BindProperty]
        public InputModel UserChange { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return RedirectToPage("./Accounts");
            }

            var userClaims = await userManager.GetClaimsAsync(user);
            this.UserChange = new InputModel
            {
                ID          = user.Id,
                Email       = user.Email,
                Username    = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Name = userManager.GetClaimsAsync(user).
                                   Result.Where(x => x.Type == ClaimTypes.GivenName).
                                   Select(x => x.Value).
                                   Single(),
                LastName = userManager.GetClaimsAsync(user).
                                       Result.Where(x => x.Type == ClaimTypes.Surname).
                                       Select(x => x.Value).
                                       Single()
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await userManager.FindByIdAsync(this.UserChange.ID);
            var claimsToAdd = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, this.UserChange.Name),
                new Claim(ClaimTypes.Surname, this.UserChange.LastName)
            };
            var oldRole = userManager.GetRolesAsync(user).Result.First();
            if (user == null)
            {
                return RedirectToPage("./Accounts");
            }

            user.Email       = this.UserChange.Email;
            user.UserName    = this.UserChange.Username;
            user.PhoneNumber = this.UserChange.PhoneNumber;
            if (oldRole != this.UserChange.Role && oldRole != "Admin")
            {
                await userManager.AddToRoleAsync(user, this.UserChange.Role);
                await userManager.RemoveFromRoleAsync(user, oldRole);
            }

            var claimName     = userManager.GetClaimsAsync(user).Result.FirstOrDefault(c => c.Type == ClaimTypes.GivenName);
            var claimLastName = userManager.GetClaimsAsync(user).Result.FirstOrDefault(c => c.Type == ClaimTypes.Surname);
            if (claimName.Value != this.UserChange.Name || claimLastName.Value != this.UserChange.LastName)
            {
                await userManager.RemoveClaimAsync(user, claimName);
                await userManager.RemoveClaimAsync(user, claimLastName);
                await userManager.AddClaimsAsync(user, claimsToAdd);
            }

            var result = await userManager.UpdateAsync(user);
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

        public class InputModel
        {
            public string ID { get; set; }

            [Required]
            [Display(Name = "Username")]
            public string Username { get; set; }

            [Required]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [Display(Name = "Role")]
            public string Role { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Display(Name = "Phone Number")]
            [Phone]
            public string PhoneNumber { get; set; }
        }
    }
}