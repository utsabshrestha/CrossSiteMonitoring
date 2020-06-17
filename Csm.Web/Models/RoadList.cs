using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Csm.Web.Models
{
    public class RoadList
    {
        public string road_code { get; set; }
        public string district { get; set; }
        public DateTime last_uploaded_date { get; set; }
    }
}
