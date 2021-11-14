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
            var pageModel = new LoginModel(_signInManager, _logger, _userManager);

            //act
            pageModel.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            var result = pageModel.OnPostAsync();

            //assert
            Assert.IsType<BadRequestResult>(result); //problema nes pas mus ne bad request grazina bet kazkokia tai nurodita message
        }

        [Fact]
        public void OnPost_IfValidModel_ReturnPage()
        {
            var options = new DbContextOptionsBuilder<LibDbContext>()
                .UseInMemoryDatabase(databaseName: "TestNewListDb").Options;
            //reikia issiaiskint kaip cia pasiekt, koks tiksliai yra vardas ir klausimas ar man isviso reikia pasiekt ja?
            //bet manau, kad reikia vistiek, bet nezinau kam realiai, jei uztenka tik response gaut ar gavau ismetama error ar
            //ar man yra atidaromas kazkoks kistas page, nebent tikrint _logger.LogInformation -> kadangi ten yra rasoma zinute jei pavyko


            //nesekmes atveju i ModelState.AddModelError prideda zinute "Invalid login attempt"


            //arrange
            var pageModel = new LoginModel(_signInManager, _logger, _userManager);
          

            InputModel input = new InputModel() //wtf kodel nepasiekia nors toj paciojdalyje 
            {
                Username = "Admin",
                Password = "Testing2@",
                RememberMe = false
            }; //as jo pasiekt taip negaliu nes ta klase yra tarp LoginModel klases tai ji yra jos viduj
            //kaip yra daromas post is to inputo pasiima tuomet username, pass ir t.t.
            // ir tuomet paduoda per _signInManager ir tikrina


            //act                
            var result = pageModel.OnPostAsync();

            //assert
            Assert.IsType<PageResult>(result);
        }

    }
}
