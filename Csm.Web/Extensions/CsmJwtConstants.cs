using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Csm.Web.Extensions
{
    public class CsmJwtConstants
    {
        public const string AuthSchemes =
            "Identity.Application" + "," + JwtBearerDefaults.AuthenticationScheme;
    }
}
