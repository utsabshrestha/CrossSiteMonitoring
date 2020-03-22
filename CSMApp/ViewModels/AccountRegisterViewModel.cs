using CSMApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMApp.ViewModels
{
    public class AccountRegisterViewModel
    {
        public AccountRegisterViewModel()
        {
            UserRegistration = new UserRegistration();
            district_all = new List<SelectListItem>();
            User = new List<ApplicationUser>();
        }
        public string PageTitle { get; set; }
        public List<SelectListItem> district_all { get; set; }
        public IEnumerable<ApplicationUser> User { get; set; }
        public UserRegistration UserRegistration { get; set; }
    }
}
