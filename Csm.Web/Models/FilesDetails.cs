using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Csm.Web.Models
{
    public class FilesDetails
    {
        public string uuid { get; set; }
        public int file_id { get; set; }
        public string form_id { get; set; }
        public string file_name { get; set; }
        public string file_note { get; set; }
        public string unique_file { get; set; }
        public string file_type { get; set; }
    }
}
