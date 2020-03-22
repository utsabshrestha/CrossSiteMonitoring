using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CSMApp.Models
{
    public class District
    {
        public int id { get; set; }
        public string district_name { get; set; }
        public string dcode { get; set; }
    }
}
