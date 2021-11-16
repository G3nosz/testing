using System;
using Xunit;
using System.Collections.Generic;
using System.Text;
using SVS.Areas.Identity.Pages.Account;
using SVS.Areas.Identity.Data;


using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace TestProject1
{
    public class IndexPageTests
    {
        private readonly ILogger<LoginModel> _logger;
        private readonly SignInManager<SVSUser> _signInManager;
        private readonly UserManager<SVSUser> _userManager;

        [Fact]
        public void OnPost_IfInvalidModel_ReturnBadRequest()
        {
            //arrange
            var loginModel = new LoginModel(_signInManager, _logger, _userManager);

            //act
            loginModel.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            var result = loginModel.OnPostAsync();

            //assert
            Assert.IsType<BadRequestResult>(result); //problema nes pas mus ne bad request grazina bet kazkokia tai nurodita message
        }

        [Fact]
        public void OnPost_IfValidModel_ReturnPage()
        {
            //arrange
            var loginModel = new LoginModel(_signInManager, _logger, _userManager);

            loginModel.Input.Username = "Admin";
            loginModel.Input.Password = "Testing2@";
            loginModel.Input.RememberMe = false;


            //act                
            var result = loginModel.OnPostAsync();

            //assert
            Assert.IsType<PageResult>(result);
        }


    }
}
