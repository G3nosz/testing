namespace SVS.Areas.Identity.Pages.Account.Manage
{
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class EmailModel : PageModel
    {
        private readonly IEmailSender           _emailSender;
        private readonly SignInManager<SVSUser> _signInManager;
        private readonly UserManager<SVSUser>   _userManager;

        public EmailModel(
            UserManager<SVSUser>   userManager,
            SignInManager<SVSUser> signInManager,
            IEmailSender           emailSender)
        {
            _userManager   = userManager;
            _signInManager = signInManager;
            _emailSender   = emailSender;
        }

        public string Username { get; set; }

        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        private async Task LoadAsync(SVSUser user)
        {
            var email = await _userManager.GetEmailAsync(user);
            this.Email = email;

            this.Input = new InputModel
            {
                NewEmail = email
            };

            this.IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
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

        public async Task<IActionResult> OnPostChangeEmailAsync()
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

            var email = await _userManager.GetEmailAsync(user);
            if (this.Input.NewEmail != email)
            {
                var code   = await _userManager.GenerateChangeEmailTokenAsync(user, this.Input.NewEmail);
                var result = await _userManager.ChangeEmailAsync(user, this.Input.NewEmail, code);
                Debug.WriteLine(result.Succeeded);
                return RedirectToPage();
            }

            this.StatusMessage = "Your email is unchanged.";
            return RedirectToPage();
        }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "New email")]
            public string NewEmail { get; set; }
        }
    }
}