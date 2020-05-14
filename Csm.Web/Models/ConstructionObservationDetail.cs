using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Csm.Web.Models
{
    public class ConstructionObservationDetail
    {
        [Key]
        public int cons_id { get; set; }
        public string form_id { get; set; }
        public string construction_type { get; set; }
        public string location { get; set; }
        public string observation_notes { get; set; }
        public string quality_rating { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public double altitude { get; set; }
        public DateTime date { get; set; }
        public int initials_id { get; set; }
        public string road_code { get; set; }
        public string location_type { get; set; }
        public double line_latitude_from { get; set; }
        public double line_longitude_from { get; set; }
        public double line_latitude_to { get; set; }
        public double line_longitude_to { get; set; }
    }
}
