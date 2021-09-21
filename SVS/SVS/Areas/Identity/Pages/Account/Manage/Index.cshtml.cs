namespace SVS.Areas.Identity.Pages.Account.Manage
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class IndexModel : PageModel
    {
        private readonly SignInManager<SVSUser> _signInManager;
        private readonly UserManager<SVSUser>   _userManager;

        public IndexModel(
            UserManager<SVSUser>   userManager,
            SignInManager<SVSUser> signInManager)
        {
            _userManager   = userManager;
            _signInManager = signInManager;
        }

        public string Username    { get; set; }
        public string FirstName   { get; set; }
        public string LastName    { get; set; }
        public string PhoneNumber { get; set; }
        public string Email       { get; set; }
        public string Role        { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        private async Task LoadAsync(SVSUser user)
        {
            var userName    = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var email       = await _userManager.GetEmailAsync(user);
            var roles       = await _userManager.GetRolesAsync(user);

            var claims = await _userManager.GetClaimsAsync(user);
            this.Username    = userName;
            this.FirstName   = claims.First(x => x.Type == ClaimTypes.GivenName).Value;
            this.LastName    = claims.First(x => x.Type == ClaimTypes.Surname).Value;
            this.PhoneNumber = phoneNumber;
            this.Email       = email;
            this.Role        = roles.FirstOrDefault();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(this.User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(this.User)}'.");
            }

            if (!this.ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            this.StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        public class InputModel
        {
        }
    }
}