using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Csm.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Csm.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;

        //private readonly RoleManager<ApplicationUser> _roleManager;
        //private readonly UserManager<ApplicationUser> _userManager;
        //, RoleManager<ApplicationUser> roleManager, UserManager<ApplicationUser> userManager

        public HomeController(ILogger<HomeController> logger)
        {
            this.logger = logger;

            //this._roleManager = roleManager;
            //this._userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Value()
        {
            //logger.LogInformation("view me in data");
            return Ok(new { message = "Uello Utsab"});
        }
        
        [Authorize]
        public IActionResult Privacy()
        {
            //throw new Exception("Error in Privacy");
            // async Task<IActionResult>
            //string[] roles = { "Admin", "Developer", "Client" };

            //foreach(var role in roles)
            //{
            //    var roleExist = await _roleManager.RoleExistsAsync(role);

            //    if(roleExist == false)
            //    {
            //        await _roleManager.CreateAsync(new IdentityRole(role));
            //    }
            //}

            //var user = await _userManager.FindByEmailAsync("utsab@iamutsab.com");

            //if(user != null)
            //{
            //    await _userManager.AddToRoleAsync(user, "Admin");
            //    await _userManager.AddToRoleAsync(user, "Developer");
            //}

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
