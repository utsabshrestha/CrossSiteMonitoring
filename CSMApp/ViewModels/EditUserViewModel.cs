using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSMApp.ViewModels
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Claims = new List<string>();
            Roles = new List<string>();
            district_all = new List<SelectListItem>();
        }

        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name ="Observer Name")]
        [Required]
        public string ObserverName { get; set; }
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
        [Required]
        [Display(Name = "Is Ranked User")]
        public bool is_ranked_user { get; set; }
        [Required]
        [Display(Name = "In Maintenance")]
        public int in_maintenance { get; set; }
        public DateTime date { get; set; }
        public string status { get; set; }

        public List<SelectListItem> district_all { get; set; }
        public List<string> Claims { get; set; }

        public IList<string> Roles { get; set; }
    }
}
