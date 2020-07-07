using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Csm.Dto.Entities
{
    public class SynchronizeDataDto
    {
        [Required]
        public string username { get; set; }
        [Required]
        public IFormFile GetFormFile{ get; set; }
    }
}
