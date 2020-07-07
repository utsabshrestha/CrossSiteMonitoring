using Csm.Domain.Config;
using Csm.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Csm.Web.ViewModels
{
    public class AccountRegisterViewModel
    {
        public AccountRegisterViewModel()
        {
            UserRegistration = new UserRegistration();
            district_all = new List<SelectListItem>();
            User = new List<ApplicationUserDomain>();
        }
        public string PageTitle { get; set; }
        public List<SelectListItem> district_all { get; set; }
        public IEnumerable<ApplicationUserDomain> User { get; set; }
        public UserRegistration UserRegistration { get; set; }
    }
}
