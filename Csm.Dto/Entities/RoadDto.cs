using System;
using System.Collections.Generic;
using System.Text;

namespace Csm.Dto.Entities
{
    public class RoadDto
    {
        public string road_code { get; set; }
        public string district { get; set; }
        public DateTime last_uploaded_date { get; set; }
    }
}
