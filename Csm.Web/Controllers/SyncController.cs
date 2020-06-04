using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Csm.Services.ServiceInterface;
using Csm.Services.ServicesAccess;
using Csm.Web.Extensions;
using Csm.Web.Models;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Csm.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = CsmJwtConstants.AuthSchemes)]
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
            if (ModelState.IsValid)
            {
                var process = await syncApi.SyncData(apiCred);
                if(process.getstatus == SyncStatus.stat.Success)
                {
                    return Ok( new { status = process.getstatus.ToString(), statusCode = process.getstatus, message = process.Message});
                }
                return BadRequest(new { status = process.getstatus.ToString(), statusCode = process.getstatus, message = process.Message });
            }
            return BadRequest(new { status = "Error", message = "Not sufficient information.", statusCode = 0 });
        }

        private async Task<bool> IsValidUsernameAndPassword(string username, string password)
        {
            var user = await userManager.FindByNameAsync(username) ?? await userManager.FindByEmailAsync(username);
            return await userManager.CheckPasswordAsync(user, password);
        }
    }
}
