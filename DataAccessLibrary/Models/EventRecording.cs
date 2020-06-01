using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models
{
    public class EventRecording
    {
        public string uuid { get; set; }
        public string district { get; set; }
        public string road_link { get; set; }
        public string river_name { get; set; }
        public Double latitude { get; set; }
        public Double longitude { get; set; }
        public string division { get; set; }
        public string events { get; set; }
        public string remarks { get; set; }
        public string observations { get; set; }
        public string msg_priority { get; set; }
        public string form_id { get; set; }
        public string date { get; set; }
        public string road_code { get; set; }
    }
}
