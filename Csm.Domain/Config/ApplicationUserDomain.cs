using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Csm.Domain.Config
{
    public class ApplicationUserDomain : IdentityUser
    {
        public string observer_name { get; set; }
        public string designation { get; set; }
        public string organization { get; set; }
        public string user_type { get; set; }
        public string district { get; set; }
        public bool is_ranked_user { get; set; }
        public int in_maintenance { get; set; }
        public DateTime date { get; set; }
        public bool status { get; set; }
    }
}
