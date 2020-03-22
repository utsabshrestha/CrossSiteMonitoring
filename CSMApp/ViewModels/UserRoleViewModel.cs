using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMApp.ViewModels
{
    public class UserRoleViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ObserverName { get; set; }
        public bool IsSelected { get; set; }
    }
}
