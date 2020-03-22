using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models
{
    public class Road
    {
        public int id { get; set; }
        public string road_code { get; set; }
        public string road_name { get; set; }
        public string district { get; set; }
        public DateTime date { get; set; }
        public DateTime uploadDate { get; set; }
    }
}
