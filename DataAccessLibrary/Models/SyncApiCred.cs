using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccessLibrary.Models
{
    public class SyncApiCred
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string filename { get; set; }
        [Required]
        public IFormFile dbfile { get; set; }
    }
}
