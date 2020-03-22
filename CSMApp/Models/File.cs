using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CSMApp.Models
{
    public class File
    {
        [Key]
        public int file_id { get; set; }
        public string form_id { get; set; }
        public string file_name { get; set; }
        public string file_note { get; set; }
        public string  unique_file { get; set; }
        public string file_type { get; set; }
        public int initials_id { get; set; }
    }
}
