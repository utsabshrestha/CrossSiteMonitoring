using Csm.Domain.SynchronizeApi.Service;
using Csm.Dto.Entities;
using Csm.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Csm.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = CsmJwtConstants.AuthSchemes)]
    [IgnoreAntiforgeryToken(Order = 1001)] //ignore csrf validation
    public class SyncController : ControllerBase
    {
        private readonly ISyncronizeService syncronizeService;

        public SyncController(ISyncronizeService syncronizeService)
        {
            this.syncronizeService = syncronizeService;
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
        public async Task<IActionResult> Post([FromForm] SyncApiCred syncApiCred)
        {
            if (ModelState.IsValid)
            {
                var Synced = await syncronizeService.Syncronize(syncApiCred);
                if (Synced)
                    return Ok(); //retun 200
                else
                    return StatusCode(500);
            }
            return BadRequest(new { message = "Not sufficient information.", statusCode = 0 }); // return 400
        }
    }
}
