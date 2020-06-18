using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Csm.Web.Data;
using Csm.Web.Models;
using Csm.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Csm.Services.ServiceInterface;

namespace Csm.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IInventory inventory;

        public object AcountRegisterViewModel { get; private set; }

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IInventory inventory)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.inventory = inventory;
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }


        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> Register()
        {
            var getDistrict = await inventory.GetDistricts();

            var district = (from dist in getDistrict
                            select new SelectListItem
                            {
                                Value = dist.district_name,
                                Text = dist.district_name
                            }).ToList();

            var users = userManager.Users.ToList();

            AccountRegisterViewModel model = new AccountRegisterViewModel()
            {
                district_all = district,
                User = userManager.Users.ToList(),
                PageTitle = "User Registration"
            };

            return View(model);
        }

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> Register(AccountRegisterViewModel ViewModel)
        {
            UserRegistration model = ViewModel.UserRegistration;

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.user_name,
                    observer_name = model.observer_name,
                    user_type = model.user_type,
                    Email = model.Email,
                    district = model.district,
                    in_maintenance = model.in_maintenance,
                    organization = model.organization,
                    designation = model.designation,
                    is_ranked_user = (bool)model.is_ranked_user,
                    date = DateTime.Now,
                    status = true
                };

                var result = await userManager.CreateAsync(user, model.Password);
                
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers", "Administration");
                    //await signInManager.SignInAsync(user, isPersistent: false);
                    //return RedirectToAction("index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            var getDistrict = await inventory.GetDistricts();

            ViewModel.district_all = (from dist in getDistrict
                                      select new SelectListItem
                                      {
                                          Value = dist.district_name,
                                          Text = dist.district_name
                                      }).ToList();

            ViewModel.User = userManager.Users.ToList();
            ViewModel.PageTitle = "User Registration";
            return View(ViewModel);
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Email) ?? await userManager.FindByEmailAsync(model.Email);

                if (user != null && (user.status == true))
                {
                    var result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

                    //var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(returnUrl))
                        {
                            return LocalRedirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("index", "Home");
                        }
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}