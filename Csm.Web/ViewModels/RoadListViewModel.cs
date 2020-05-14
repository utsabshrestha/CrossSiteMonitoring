using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Csm.Web.ViewModels
{
    public class RoadListViewModel
    {
        public RoadListViewModel()
        {
            district_all = new List<SelectListItem>();
            RoadList = new List<Road>();
        }
        public List<Road> RoadList { get; set; }
        public List<SelectListItem> district_all { get; set; }
        public string districtSelected { get; set; }
    }
}
