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
using Microsoft.Extensions.Logging;

namespace Csm.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IInventory inventory;
        private readonly IMapper mapper;
        private readonly ISyncApi syncApi;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<DashboardController> logger;

        public DashboardController(UserManager<ApplicationUser> userManager, IInventory inventory, 
            IMapper mapper,
            ISyncApi syncApi,
            SignInManager<ApplicationUser> signInManager,
            ILogger<DashboardController> logger)
        {
            this.userManager = userManager;
            this.inventory = inventory;
            this.mapper = mapper;
            this.syncApi = syncApi;
            this.signInManager = signInManager;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> ListRoads(string redirect)
        {
            if (redirect != "sites" && redirect != "mysites")
                return RedirectToAction("HttpStatusCodeHandler", "Error");

            var user = await GetCurrentUserAsync();
            IEnumerable<Road> roaddetails = await inventory.GetRoadsList(redirect, user?.Email);
            IEnumerable<District> districts = await inventory.GetDistrictList(redirect, user?.Email);

            var districtItems =  (from dis in districts select new SelectListItem { Value = dis.district_name, Text = dis.district_name }).ToList();
            var roadlists = mapper.Map<IEnumerable<Road>, IEnumerable<RoadList>>(roaddetails);
            var model = new RoadListViewModel
            {
                district_all = districtItems,
                RoadList = roadlists
            };

            ViewData["district"] = "none";
            ViewData["Redirection"] = redirect;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ListRoads(string districtSelected, string redirect)
        {
            if (String.IsNullOrEmpty(districtSelected))
            {
                return RedirectToAction("ListRoads",new { redirect = redirect });
            }
            if (redirect != "sites" && redirect != "mysites")
                return RedirectToAction("HttpStatusCodeHandler", "Error");

            var user = await GetCurrentUserAsync();
            IEnumerable<Road> roaddetails = await inventory.GetRoadsList(redirect, user?.Email, districtSelected); 
            IEnumerable<District> districts = await inventory.GetDistrictList(redirect, user?.Email);

            var districtItems = (from dis in districts select new SelectListItem { Value = dis.district_name, Text = dis.district_name }).ToList();
            
            var roadlists = mapper.Map<IEnumerable<Road>, IEnumerable<RoadList>>(roaddetails);
            var model = new RoadListViewModel
            {
                districtSelected = districtSelected,
                district_all = districtItems,
                RoadList = roadlists
            };
            ViewData["district"] = "exist";
            ViewData["Redirection"] = redirect;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ListRoadDetail(RoadList model, string districtStatus, string redirect)
        {
            IEnumerable<RoadDetails> roaddetails = null;

            if (districtStatus == "none")
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
            ViewData["Redirection"] = redirect;
            return View(roadDetail);
        }

        [HttpPost]
        public async Task<IActionResult> ViewReport(string form_id, string road_code, DateTime date, string observer_email, string redirect)
        {

            GenericReport<Inital, ConstructionObservation, Files> report =
                await inventory.GetWholeReport<Inital, ConstructionObservation, Files>(form_id, road_code);

            FullReport fullReport = new FullReport
            {
                inital = mapper.Map<IEnumerable<Inital>,IEnumerable<InitialsDetails>>(report.GetInitial),
                constructionObservations = mapper.Map<IEnumerable<ConstructionObservation>,IEnumerable<ConstructionObservationDetail>>(report.GetConstruction),
                files = mapper.Map<IEnumerable<Files>,IEnumerable<FilesDetails>>(report.GetFiles)
            };

            if (report.GetInitial.Any())
            {
                ViewData["district"] = report.GetInitial.ElementAt(0).district;
                ViewData["road_code"] = report.GetInitial.ElementAt(0).road_code;
            }
            ViewData["Redirection"] = redirect;
            return View(fullReport);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeReportStatus([FromBody] SelectedeReprotData road)
        {
            var user = await GetCurrentUserAsync();
            if (!User.IsInRole("Admin") && user?.Email != road.observer_email)
            {
                return Unauthorized(new { message = "Oops Access Denied."});
            }
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
        public async Task<IActionResult> EditConstructionReport(SelectedeReprotData selectedeReprotData, string redirect)
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
                inital = mapper.Map<IEnumerable<Inital>, IEnumerable<InitialsDetails>>(report.GetInitial),
                constructionObservations = mapper.Map<IEnumerable<ConstructionObservation>, IEnumerable<ConstructionObservationDetail>>(report.GetConstruction),
                files = mapper.Map<IEnumerable<Files>, IEnumerable<FilesDetails>>(report.GetFiles)
            };
            if (report.GetInitial.Any())
            {
                ViewData["district"] = report.GetInitial.ElementAt(0).district;
                ViewData["road_code"] = report.GetInitial.ElementAt(0).road_code;
            }
            ViewData["message"] = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/Dashboard/UpdateObservations";
            ViewData["Redirection"] = redirect;
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
                return BadRequest(new { message = "Oops Access Denied." });
            }

            logger.LogInformation("The Report {report} is Deleted by the user {user}", selectedeReprotData.form_id, user.Email);
            
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
