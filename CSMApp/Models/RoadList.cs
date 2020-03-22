using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CSMApp.Models
{
    public class RoadList
    {
        public int id { get; set; }
        public string road_code { get; set; }
        public string road_name { get; set; }
        public string district { get; set; }
        public DateTime date { get; set; }
        public DateTime uploadDate { get; set; }
    }
}
