using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CSMApp.Models;
using CSMApp.ViewModels;
using DataAccessLibrary.DataAccess;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CSMApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IInventory inventory;
        private readonly IMapper mapper;

        public DashboardController(UserManager<ApplicationUser> userManager, IInventory inventory, IMapper mapper)
        {
            this.userManager = userManager;
            this.inventory = inventory;
            this.mapper = mapper;
        }


        [HttpGet]
        public IActionResult ListRoads()
        {
            var roaddetails = inventory.GetRoads();
            var districts = inventory.GetDistricts();

            var districtItems = (from dis in districts select new SelectListItem  { Value = dis.district_name, Text = dis.district_name }).ToList();

            List<RoadList> roadlists = new List<RoadList>();

            foreach (var road in roaddetails)
            {
                roadlists.Add(mapper.Map<RoadList>(road));
            }

            var model = new RoadListViewModel
            {
                district_all = districtItems,
                RoadList = roadlists
            };

            return View(model);
        } 
        
        [HttpPost]
        public IActionResult ListRoads(string districtSelected)
        {
            if (String.IsNullOrEmpty(districtSelected))
            {
                return RedirectToAction("ListRoads");
            }

            var roaddetails = inventory.GetRoads(districtSelected);
            var districts = inventory.GetDistricts();

            var districtItems = (from dis in districts select new SelectListItem { Value = dis.district_name, Text = dis.district_name }).ToList();

            List<RoadList> roadlists = new List<RoadList>();

            foreach (var road in roaddetails)
            {
                roadlists.Add(mapper.Map<RoadList>(road));
            }

            var model = new RoadListViewModel
            {
                districtSelected = districtSelected,
                district_all = districtItems,
                RoadList = roadlists
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult ListRoadDetail(RoadList model)
        {
            List<DetailRoadList> roadDetail = new List<DetailRoadList>();
            List<RoadDetails> roaddetails = null;

            if (string.IsNullOrEmpty(model.district))
            {
                 roaddetails = inventory.GetRoadDetails(model.road_code);
            }
            else
            {
                 roaddetails = inventory.GetRoadDetails(model.road_code, model.district);
            }


            foreach (var roaddetail in roaddetails)
            {
                roadDetail.Add(mapper.Map<DetailRoadList>(roaddetail));
            }

            return View(roadDetail);
        }

        [HttpPost]
        public IActionResult ViewReport(string road_code, DateTime date, string observer_email)
        {
            List<ReportDataModel> reportList = inventory.GetReportDataList(road_code, date, observer_email);
            List<ReportViewDataModel> reportViewDataModel = new List<ReportViewDataModel>();

            foreach(var roadData in reportList)
            {
                reportViewDataModel.Add(mapper.Map<ReportViewDataModel>(roadData));
            }

            ReportViewModel model = new ReportViewModel { ReportViewDataModel = reportViewDataModel };

            return View(model);
        }

    }
}