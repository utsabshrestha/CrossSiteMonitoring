using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Csm.Web.Extensions
{
    public class CsmSettings
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
