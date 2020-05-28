using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Csm.Services.ServiceInterface;
using Csm.Web.Models;
using Csm.Web.ViewModels;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Csm.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IInventory inventory;
        private readonly IMapper mapper;
        private readonly ISyncApi syncApi;

        public DashboardController(UserManager<ApplicationUser> userManager, IInventory inventory, IMapper mapper, ISyncApi syncApi)
        {
            this.userManager = userManager;
            this.inventory = inventory;
            this.mapper = mapper;
            this.syncApi = syncApi;
        }


        [HttpGet]
        public async Task<IActionResult> ListRoads()
        {
            var roaddetails = await inventory.GetRoads();
            var districts = await inventory.GetDistricts();
            var districtItems = (from dis in districts select new SelectListItem  { Value = dis.district_name, Text = dis.district_name }).ToList();

            //List<RoadList> roadlists = new List<RoadList>();

            //foreach (var road in roaddetails)
            //{
            //    roadlists.Add(mapper.Map<RoadList>(road));
            //}

            var model = new RoadListViewModel
            {
                district_all = districtItems,
                RoadList = roaddetails.ToList()
            };

            return View(model);
        } 
        
        [HttpPost]
        public async Task<IActionResult> ListRoads(string districtSelected)
        {
            if (String.IsNullOrEmpty(districtSelected))
            {
                return RedirectToAction("ListRoads");
            }

            var roaddetails = await inventory.GetRoads(districtSelected);
            var districts = await inventory.GetDistricts();

            var districtItems = (from dis in districts select new SelectListItem { Value = dis.district_name, Text = dis.district_name }).ToList();

            //List<RoadList> roadlists = new List<RoadList>();

            //foreach (var road in roaddetails)
            //{
            //    roadlists.Add(mapper.Map<RoadList>(road));
            //}

            var model = new RoadListViewModel
            {
                districtSelected = districtSelected,
                district_all = districtItems,
                RoadList = roaddetails.ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ListRoadDetail(RoadList model)
        {
            List<DetailRoadList> roadDetail = new List<DetailRoadList>();
            IEnumerable<RoadDetails> roaddetails = null;

            if (string.IsNullOrEmpty(model.district))
            {
                 roaddetails = await inventory.GetRoadDetails(model.road_code);
            }
            else
            {
                 roaddetails = await inventory.GetRoadDetails(model.road_code, model.district);
            }


            foreach (var roaddetail in roaddetails)
            {
                roadDetail.Add(mapper.Map<DetailRoadList>(roaddetail));
            }

            return View(roadDetail);
        }

        [HttpPost]
        public async Task<IActionResult> ViewReport(string road_code, DateTime date, string observer_email)
        {
            IEnumerable<ReportDataModel> reportList = await inventory.GetReportDataList(road_code, date, observer_email);
            IList<ReportModel> reportViewModel = new List<ReportModel>();

            foreach(var roadData in reportList)
            {
                reportViewModel.Add(mapper.Map<ReportModel>(roadData));
            }

            ReportViewModel model = new ReportViewModel { ReportData = reportViewModel };

            return View(model);
        }

        [HttpGet]
        public IActionResult test() 
        {
            syncApi.Inst();
            return Ok();
        }
    }
}