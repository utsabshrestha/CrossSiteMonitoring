using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Csm.Web.Models
{
    public class ConstructionDataModel
    {
        public string uuid { get; set; }
        public string form_id { get; set; }
        public string construction_type { get; set; }
        public string location { get; set; }
        public string observation_notes { get; set; }
        public string quality_rating { get; set; }
        public string road_code { get; set; }
        public string location_type { get; set; }
        public string observer_email { get; set; }

    }
}
