﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Csm.Web.Models
{
    public class TokenCred
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
    }
}
