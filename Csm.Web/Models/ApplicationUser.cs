using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Csm.Web.Models
{
    public class ApplicationUser : IdentityUser
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