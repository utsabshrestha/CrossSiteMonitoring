using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Csm.Services.ServiceInterface;
using Csm.Services.ServicesAccess;
using Csm.Web.Models;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Csm.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyncController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ISyncApi syncApi;

        public SyncController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ISyncApi syncApi)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.syncApi = syncApi;
        }

        // GET: api/Sync
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Sync/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Sync
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] SyncApiCred apiCred)
        {
            if (ModelState.IsValid && ( await Login(apiCred.username, apiCred.password)))
            {
                var process = await syncApi.SyncData(apiCred);
                if(process.getstatus == SyncStatus.stat.Success)
                {
                    return Ok( new { status = process.getstatus.ToString(), statusCode = process.getstatus, message = process.Message});
                }
                return BadRequest(new { status = process.getstatus.ToString(), statusCode = process.getstatus, message = process.Message });
            }
            return BadRequest(new { status = "Error", message = "Invalid attempt to Login." });
        }

        private async Task<bool> Login(string username, string password)
        {
            var user = await userManager.FindByNameAsync(username) ?? await userManager.FindByEmailAsync(username);
             if(user != null)
            {
                var result = await signInManager.PasswordSignInAsync(user, password, false, false);
                if (result.Succeeded)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
