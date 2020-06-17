using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Csm.Web.ViewModels
{
    public class DetailRoadList
    {
        public int ini_id { get; set; }
        public string form_id { get; set; }
        public string road_code { get; set; }
        public string road_name { get; set; }
        public string district { get; set; }
        public DateTime date { get; set; }
        public DateTime uploaded_date { get; set; }
        public string observer_name { get; set; }
        public string observer_email { get; set; }
        public string designation { get; set; }
        public int report_status { get; set; }
    }
}

