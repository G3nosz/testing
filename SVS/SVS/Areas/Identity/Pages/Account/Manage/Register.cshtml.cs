namespace SVS.Areas.Identity.Pages.Account.Manage
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Extensions.Logging;

    [Authorize(Roles = "Admin")]
    public class RegisterModel : PageModel
    {
        private readonly IEmailSender              _emailSender;
        private readonly ILogger<RegisterModel>    _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<SVSUser>    _signInManager;
        private readonly UserManager<SVSUser>      _userManager;

        public List<SelectListItem> Roles = new List<SelectListItem>
        {
            new SelectListItem {Text = "Employee", Value = "Employee"},
            new SelectListItem {Text = "Driver", Value   = "Driver"}
        };

        public RegisterModel(
            UserManager<SVSUser>      userManager,
            SignInManager<SVSUser>    signInManager,
            ILogger<RegisterModel>    logger,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager   = userManager;
            _signInManager = signInManager;
            _logger        = logger;
            _roleManager   = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            this.ReturnUrl      = returnUrl;
            this.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl           ??=this.Url.Content("~/");
            this.ExternalLogins =  (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (!this.ModelState.IsValid)
            {
                return Page();
            }

            var roleList = new[] {"Admin", "Employee", "Driver"};
            foreach (var roleName in roleList)
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var claimsToAdd = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, this.Input.Name),
                new Claim(ClaimTypes.Surname, this.Input.LastName)
            };

            var user = new SVSUser
                {UserName = this.Input.Username, Email = this.Input.Email, PhoneNumber = this.Input.PhoneNumber};
            var result = await _userManager.CreateAsync(user, this.Input.Password);
            if (result.Succeeded)
            {
                await _userManager.AddClaimsAsync(user, claimsToAdd);
                if (this.Input.Username == "Admin")
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, this.Input.Role);
                }

                return RedirectToPage("./Added", new {User = user.UserName});
            }

            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error.Description);
            }


            return Page();
        }

        public class InputModel
        {
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

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name                     = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }
    }
}