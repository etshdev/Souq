using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Souq.DataAccessLayer;
using Souq.Tables;
using Souq.ViewsModels;

namespace Souq.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRepository<User> _repoUser;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(IRepository<User> repository, UserManager<IdentityUser> userManager,
                                        SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _repoUser = repository;
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

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserViewModels model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var newUser = new IdentityUser()
                    {
                        UserName = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        Email = model.Email
                    };

                    var result = await _userManager.CreateAsync(newUser, model.Password);
                    if(result.Succeeded)
                    {
                        var user = new User()
                        {
                            FullName=model.FullName,
                            Address = model.Address,
                            AppUserId = newUser.Id
                        };
                        _repoUser.Insert(user);
                        await _userManager.AddToRoleAsync(newUser, "BuyerRole");
                        await _signInManager.SignInAsync(newUser, true);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }

                }

                return View(model);

            }
            catch (Exception e)
            {

                return Content(e.Message);
            }
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _signInManager.PasswordSignInAsync(model.Email,
                        model.Password, model.RememberMe, false);

                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                ModelState.AddModelError("", "Invalid login attempt");
                return View(model);
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }
        }

        [HttpGet]
        public IActionResult SignUpSeller()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUpSeller(UserViewModels model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newUser = new IdentityUser()
                    {
                        UserName = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        Email = model.Email
                    };

                    var result = await _userManager.CreateAsync(newUser, model.Password);
                    if (result.Succeeded)
                    {
                        var user = new User()
                        {
                            FullName=model.FullName,
                            Address = model.Address,
                            AppUserId = newUser.Id
                        };
                        _repoUser.Insert(user);
                        await _userManager.AddToRoleAsync(newUser, "SellerRole");
                        await _signInManager.SignInAsync(newUser, true);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }

                }

                return View(model);

            }
            catch (Exception e)
            {

                return Content(e.Message);
            }
        }



        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
