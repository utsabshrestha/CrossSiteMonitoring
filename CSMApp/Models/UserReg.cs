using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CSMApp.Models
{
    public class UserReg
    {
        [Key]
        public int user_id { get; set; }
        public string observer_name { get; set; }
        public string designation { get; set; }
        public string organization { get; set; }
        public string email { get; set; }
        public string user_type { get; set; }
        public string district { get; set; }
    }
}
