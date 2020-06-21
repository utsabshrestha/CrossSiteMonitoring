using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Csm.Services.ServiceInterface;
using Csm.Web.Models;
using Csm.Web.ViewModels;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Authorization;
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
            var districtItems = (from dis in districts select new SelectListItem { Value = dis.district_name, Text = dis.district_name }).ToList();

            var roadlists = mapper.Map<IEnumerable<Road>, IEnumerable<RoadList>>(roaddetails);

            var model = new RoadListViewModel
            {
                district_all = districtItems,
                RoadList = roadlists
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

            var roadlists = mapper.Map<IEnumerable<Road>, IEnumerable<RoadList>>(roaddetails);

            var model = new RoadListViewModel
            {
                districtSelected = districtSelected,
                district_all = districtItems,
                RoadList = roadlists
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ListRoadDetail(RoadList model)
        {
            IEnumerable<RoadDetails> roaddetails = null;

            if (string.IsNullOrEmpty(model.district))
            {
                roaddetails = await inventory.GetRoadDetails(model.road_code);
            }
            else
            {
                roaddetails = await inventory.GetRoadDetails(model.road_code, model.district);
            }

            var roadDetail = mapper.Map<IEnumerable<RoadDetails>, IEnumerable<DetailRoadList>>(roaddetails);

            ViewData["message"] = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/Dashboard/ChangeReportStatus";
            ViewData["message2"] = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/Dashboard/DeleteReport";
            ViewData["district"] = model.district;

            return View(roadDetail);
        }

        [HttpPost]
        public async Task<IActionResult> ViewReport(string form_id, string road_code, DateTime date, string observer_email)
        {

            GenericReport<Inital, ConstructionObservation, Files> report =
                await inventory.GetWholeReport<Inital, ConstructionObservation, Files>(form_id, road_code);

            FullReport fullReport = new FullReport
            {
                inital = report.GetInitial,
                constructionObservations = report.GetConstruction,
                files = report.GetFiles
            };

            if (report.GetInitial.Any())
            {
                ViewData["district"] = report.GetInitial.ElementAt(0).district;
                ViewData["road_code"] = report.GetInitial.ElementAt(0).road_code;
            }

            return View(fullReport);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeReportStatus([FromBody] SelectedeReprotData road)
        {
            var user = await GetCurrentUserAsync();

            if (!User.IsInRole("Admin") && user?.Email != road.observer_email)
            {
                return Unauthorized();
            }
            //DateTime date = Convert.ToDateTime(road.date);

            IEnumerable<Inital> InitialsDetails = await inventory.CheckReportEmail( road.form_id ,road.road_code, road.observer_email);

            if (InitialsDetails.Any())
            {
                var update = await inventory.UpdateReportStatus(road.form_id, road.road_code, road.observer_email);

                if (update == 1)
                {
                    return Ok(new { status = 1, message = "The Report is Finalized." });
                }
                else
                {
                    return BadRequest(new { status = 0, message = "Error occured while finalizing the Report." });
                }
            }
            else
            {
                return NotFound(new { status = 0, message = "The Report doesn't Exist at the moment!" });
            }
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => userManager.GetUserAsync(HttpContext.User);

        [HttpPost]
        public async Task<IActionResult> EditConstructionReport(SelectedeReprotData selectedeReprotData)
        {
            var user = await GetCurrentUserAsync();

            if (!User.IsInRole("Admin") && user?.Email != selectedeReprotData.observer_email)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            GenericReport<Inital, ConstructionObservation, Files> report = 
                await inventory.GetWholeReport<Inital, ConstructionObservation, Files>(selectedeReprotData.form_id, selectedeReprotData.road_code);

            FullReport fullReport = new FullReport
            {
                inital = report.GetInitial,
                constructionObservations = report.GetConstruction,
                files = report.GetFiles
            };

            if (report.GetInitial.Any())
            {
                ViewData["district"] = report.GetInitial.ElementAt(0).district;
                ViewData["road_code"] = report.GetInitial.ElementAt(0).road_code;
            }

            ViewData["message"] = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/Dashboard/UpdateObservations";

            return View(fullReport);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateObservations([FromBody] ConstructionDataModel constructionObservation)
        {
            IEnumerable<Inital> InitialsDetails = await inventory.CheckReportEmail(constructionObservation.uuid, constructionObservation.road_code, constructionObservation.observer_email);
            var user = await GetCurrentUserAsync();
            if (!InitialsDetails.Any())
            {
                return BadRequest(new { message = "The Report does not exist, cannot update." });
            }

            if (!User.IsInRole("Admin") && user.Email != InitialsDetails?.ElementAt(0).observer_email)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            ConstructionObservation param = new ConstructionObservation
            {
                construction_type = constructionObservation.construction_type,
                location = constructionObservation.location,
                observation_notes = constructionObservation.observation_notes,
                quality_rating = constructionObservation.quality_rating,
                form_id = constructionObservation.form_id
            };

            var status = await inventory.UpdateConstructionObservation(param);
            if (status == 1)
            {
                return Ok(new { message = "The Report has been updated." });
            }
            else
            {
                return BadRequest(new { message = "Error Occured while updating the Report." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteReport([FromBody] SelectedeReprotData selectedeReprotData)
        {
            var user = await GetCurrentUserAsync();
            if (!User.IsInRole("Admin") && user?.Email != selectedeReprotData.observer_email)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var status = await inventory.DeleteReportObservation(selectedeReprotData.form_id, selectedeReprotData.road_code);
            if (status)
            {
                return Ok(new { message = $"The Report with road code {selectedeReprotData.road_code} and date time {selectedeReprotData.date} is deleted." });
            }
            return BadRequest(new { message = "Error occured while deleting the Report." });
         }

        [HttpGet]
        public async Task<IActionResult> test()
        {
            var LoggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name); // will give the user's userName
            var applicationUser = await userManager.GetUserAsync(User);

            //var LoggedInUser = 
            return Ok();
        }
    }
}
