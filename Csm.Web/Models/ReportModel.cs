using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Csm.Web.Models
{
    public class ReportModel
    {
        public int ini_id { get; set; }
        [Display(Name ="Road Code")]
        public string road_code { get; set; }
        [Display(Name ="Road Name")]
        public string road_name { get; set; }
        [Display(Name ="District")]
        public string district { get; set; }
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime date { get; set; }
        [Display(Name = "Observer Name")]
        public string observer_name { get; set; }
        [Display(Name = "Observer Email")]
        public string observer_email { get; set; }
        [Display(Name = "Designation")]
        public string designation { get; set; }
        public int cons_id { get; set; }
        public string form_id { get; set; }
        public string construction_type { get; set; }
        public string location_type { get; set; }
        public string location { get; set; }
        public string observation_notes { get; set; }
        public string quality_rating { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public double line_latitude_from { get; set; }
        public double line_longitude_from { get; set; }
        public double line_latitude_to { get; set; }
        public double line_longitude_to { get; set; }
        public double altitude { get; set; }
        public int file_id { get; set; }
        public string file_name { get; set; }
        public string file_note { get; set; }
        public string unique_file { get; set; }
        public string file_type { get; set; }
    }
}
