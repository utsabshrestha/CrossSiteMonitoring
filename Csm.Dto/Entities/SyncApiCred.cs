using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Http;
namespace Csm.Dto.Entities
{
    public class SyncApiCred
    {
        [Required]
        public string username { get; set; }
        [Required]
        public IFormFile formFile { get; set; }
    }
}