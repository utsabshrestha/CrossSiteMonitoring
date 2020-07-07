using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Csm.Domain.Config;
using Csm.Web.Data;
using Csm.Web.Extensions;
using Csm.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Csm.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUserDomain> userManager;
        private readonly CsmSettings settings;

        public TokenController(ApplicationDbContext context, UserManager<ApplicationUserDomain> userManager, IOptions<CsmSettings> options)
        {
            this.context = context;
            this.userManager = userManager;
            this.settings = options.Value;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Give me sunshine.", "Give me some rain." };
        }

        [HttpPost]
        public async Task<IActionResult> Post(TokenCred tokenCred)
        {
            if (tokenCred.username != null && tokenCred.password != null && ModelState.IsValid)
            {
                if (await IsValidUsernamePassword(tokenCred.username, tokenCred.password))
                {
                    return new ObjectResult(await GenerateToken(tokenCred.username));
                }
                else
                {
                    return BadRequest(new { message = "Username or password is incorrect" });
                }
            }
            return BadRequest(new { message = "Pleas provide correct information to retrieve the Token." });
        }

        private async Task<bool> IsValidUsernamePassword(string username, string password)
        {
            var user = await userManager.FindByEmailAsync(username) ?? await userManager.FindByNameAsync(username);
            if (await userManager.CheckPasswordAsync(user, password) && user.status == true)
                return true;
            else
                return false;
        }

        private async Task<dynamic> GenerateToken(string username)
        {
            var user = await userManager.FindByEmailAsync(username) ?? await userManager.FindByNameAsync(username);
            var roles = from ur in context.UserRoles
                        join r in context.Roles on ur.RoleId equals r.Id
                        where ur.UserId == user.Id
                        select new { ur.UserId, ur.RoleId, r.Name };


            var claims = new List<Claim>
            {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, user.Id),
            new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //var token = new JwtSecurityToken(
            //    new JwtHeader(creds),
            //    new JwtPayload(claims)
            //);

            var token2 = new JwtSecurityToken(
                    settings.Issuer,
                    settings.Audience,
                    claims,
                    expires: DateTime.UtcNow.AddDays(1),
                    signingCredentials: creds
                );

            var ouput = new
            {
                access_token = new JwtSecurityTokenHandler().WriteToken(token2),
                UserName = username,
                expiration = token2.ValidTo
            };

            return ouput;
        }
    }
}


//TODO: Some Insights.
//Some Insights.
//FromForm can only recieve form-data not json and
//FromBody can only recieve Json data not form-data.
//if Json is sent to FromForm the parameters recieved will be null
//and if form-data is sent to FromBody then the Request will be rejected with 
//status code 415 Unsupported Media Type
//***

//TODO: SWAGGER ERROR FIXED.
//Swagger won't work if the controller don't have the http attributes.