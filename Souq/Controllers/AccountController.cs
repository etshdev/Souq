using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Souq.DataAccessLayer;
using Souq.Tables;

namespace Souq.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<IdentityUser> userManager,
                                        SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> AddAllRoles(string Secret)
        {
            try
            {
                if (Secret == "1234")
                {
                    IdentityRole AdminRole = new IdentityRole() { Name = "AdminRole" };
                    IdentityRole SellerRole = new IdentityRole() { Name = "SellerRole" };
                    IdentityRole BuyerRole = new IdentityRole() { Name = "BuyerRole" };

                    var AdminResult = await _roleManager.CreateAsync(AdminRole);
                    var SellerResult = await _roleManager.CreateAsync(SellerRole);
                    var BuyerResult = await _roleManager.CreateAsync(BuyerRole);
                    if (AdminResult.Succeeded && SellerResult.Succeeded && BuyerResult.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");

                    }
                    else
                        return Content("Something worng happened");
                }
                else
                    return Content("Wrong Secret");
            }
            catch (Exception e)
            {

                return Content(e.Message);
            }
        }


      
    }
}
