namespace SVS.Areas.Identity.Pages.Account
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    [Authorize(Roles = "Nobody")]
    public class RegisterModels : PageModel
    {
    }
}