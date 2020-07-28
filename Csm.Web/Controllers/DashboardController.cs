using Csm.Domain.Config;
using Csm.Domain.Services;
using Csm.Dto.Entities;
using Csm.Web.Models;
using Csm.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Csm.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUserDomain> userManager;
        private readonly IEnumerable<IDashboard> dashboard;

        public DashboardController
        (
        UserManager<ApplicationUserDomain> userManager,
        IEnumerable<IDashboard> dashboard
        )
        {
            this.userManager = userManager;
            this.dashboard = dashboard;
        }

        [HttpGet]
        public async Task<IActionResult> ListRoads(string redirect)
        {
            if (redirect != "sites" && redirect != "mysites")
                return RedirectToAction("HttpStatusCodeHandler", "Error");

            var repository = dashboard.FirstOrDefault(repo => repo.User == redirect);
            IEnumerable<RoadDto> roadDtos = await repository.getRoads();
            IEnumerable<DistrictDto> districtDtos = await repository.getDistrict();

            var districtItems = (from dis in districtDtos select new SelectListItem { Value = dis.district_name, Text = dis.district_name }).ToList();
            var model = new RoadListViewModel
            {
                district_all = districtItems,
                RoadList = roadDtos
            };
            ViewData["district"] = "none";
            ViewData["Redirection"] = redirect;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ListRoads(string districtSelected, string redirect)
        {
            if (String.IsNullOrEmpty(districtSelected))
                return RedirectToAction("ListRoads", new { redirect = redirect });

            if (redirect != "sites" && redirect != "mysites")
                return RedirectToAction("HttpStatusCodeHandler", "Error");

            var repository = dashboard.FirstOrDefault(repo => repo.User == redirect);
            IEnumerable<RoadDto> roadDtos = await repository.getRoads(districtSelected);
            IEnumerable<DistrictDto> districtDtos = await repository.getDistrict();

            var districtItems = (from dis in districtDtos select new SelectListItem { Value = dis.district_name, Text = dis.district_name }).ToList();

            var model = new RoadListViewModel
            {
                districtSelected = districtSelected,
                district_all = districtItems,
                RoadList = roadDtos
            };
            ViewData["district"] = "exist";
            ViewData["Redirection"] = redirect;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ListRoadDetail(RoadList model, string districtStatus, string redirect)
        {
            var repository = dashboard.FirstOrDefault(repo => repo.User == redirect);
            IEnumerable<RoadDetailsDto> roadDetailsDtos = await repository.GetRoadDetail(model.road_code, model.district);

            ViewData["message"] = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/ReportService/ChangeReportStatus";
            ViewData["message2"] = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/ReportService/DeleteReport";
            ViewData["district"] = model.district;
            ViewData["Redirection"] = redirect;

            return View(roadDetailsDtos);
        }

        [HttpGet]
        public IActionResult MeetJitsi()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> test()
        {
            var LoggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name); // will give the user's userName
            var applicationUser = await userManager.GetUserAsync(User);
            var email = applicationUser.Email;
            var status = await dashboard.FirstOrDefault(x => x.User == "mysites").getDistrict();
            return Ok();
        }
    }
}


//TODO: don't implement disposal for dal.dataacces - Completed!