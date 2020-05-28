using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models
{
    public class SyncApiCred
    {
        public string username { get; set; }
        public string filename { get; set; }
        public string password { get; set; }
        public IFormFile dbfile { get; set; }
    }
}
