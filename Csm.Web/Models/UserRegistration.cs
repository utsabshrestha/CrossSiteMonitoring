using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Csm.Web.Models
{
    public class UserRegistration
    {
        [Required]
        [Display(Name = "Observer Name")]
        public string observer_name { get; set; }
        [Required]
        [Display(Name ="User Name")]
        public string user_name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password",
        ErrorMessage = "Passwrod and Confirmations passwrod do not match.")]
        public string ConfirmPassword { get; set; }
        [Required]
        [Display(Name = "Designation")]
        public string designation { get; set; }
        [Required]
        [Display(Name = "Organization")]
        public string organization { get; set; }
        [Required]
        [Display(Name = "User Type")]
        public string user_type { get; set; }
        [Required]
        [Display(Name = "District")]
        public string district { get; set; }
        [Display(Name = "Is Ranked User")]
        public bool is_ranked_user { get; set; }
        [Display(Name = "Roads In Maintenance")]
        public int in_maintenance { get; set; }
    }
}
