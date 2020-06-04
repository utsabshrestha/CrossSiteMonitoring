using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Csm.Web.Data;
using Csm.Web.Extensions;
using Csm.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Csm.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly CsmSettings settings;

        public TokenController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IOptions<CsmSettings> options)
        {
            this.context = context;
            this.userManager = userManager;
            this.settings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> Create(string username, string password)
        {
            if (await IsValidUsernamePassword(username, password))
            {
                return new ObjectResult(await GenerateToken(username));
            }
            else
            {
                return BadRequest(new { message = "Username or password is incorrect"});
            }
        }

        private async Task<bool> IsValidUsernamePassword(string username, string password)
        {
            var user = await userManager.FindByEmailAsync(username) ?? await userManager.FindByNameAsync(username);
            return await userManager.CheckPasswordAsync(user, password);
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
                    expires: DateTime.UtcNow.AddHours(3),
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