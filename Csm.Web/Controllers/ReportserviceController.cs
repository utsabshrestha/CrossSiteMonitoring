using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Csm.Domain.Config;
using Csm.Domain.Services;
using Csm.Dto.Entities;
using Csm.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Csm.Web.Controllers
{

    [Authorize]
    public class ReportserviceController : Controller
    {
        private readonly UserManager<ApplicationUserDomain> userManager;
        private readonly IReportServices reportServices;
        private readonly ILogger<ReportserviceController> logger;

        public ReportserviceController(
            UserManager<ApplicationUserDomain> userManager,
            IReportServices reportServices,
            ILogger<ReportserviceController> logger
            )
        {
            this.userManager = userManager;
            this.reportServices = reportServices;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> ChangeReportStatus([FromBody] SelectedeReprotData road)
        {
            var user = await userManager.GetUserAsync(User);
            if (!User.IsInRole("Admin") && user?.Email != road.observer_email)
                return Unauthorized(new { message = "Oops Access Denied." });

            bool reportChangedStatus = await reportServices.changeReportStatus(road.form_id, road.road_code, road.observer_email);

            if (reportChangedStatus)
                return Ok(new { status = 1, message = "The Report is Finalized." });
            else
                return BadRequest(new { status = 0, message = "Error occured while finalizing the Report." });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateObservations([FromBody] ConstructionDataModel constructionObservation)
        {
            var user = await userManager.GetUserAsync(User);
            if (!User.IsInRole("Admin") && user.Email != constructionObservation.observer_email)
                return RedirectToAction("AccessDenied", "Account");

            ConstructionObservationDetail construction = new ConstructionObservationDetail
            {
                construction_type = constructionObservation.construction_type,
                location = constructionObservation.location,
                observation_notes = constructionObservation.observation_notes,
                quality_rating = constructionObservation.quality_rating,
                form_id = constructionObservation.form_id
            };

            bool ReportUpdateStatus = await reportServices.UpdateConstructionObservation(construction);

            if (ReportUpdateStatus)
                return Ok(new { message = "The Report has been updated." });
            else
                return BadRequest(new { message = "Error Occured while updating the Report." });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteReport([FromBody] SelectedeReprotData selectedeReprotData)
        {
            var user = await userManager.GetUserAsync(User);
            if (!User.IsInRole("Admin") && user?.Email != selectedeReprotData.observer_email)
                return BadRequest(new { message = "Oops Access Denied." });

            logger.LogInformation("The Report {report} is Deleted by the user {user}", selectedeReprotData.form_id, user.Email);

            bool ReportDeleteStatus = await reportServices.DeleteReport(selectedeReprotData.form_id, selectedeReprotData.road_code);
            if (ReportDeleteStatus)
                return Ok(new { message = $"The Report with road code {selectedeReprotData.road_code} and date time {selectedeReprotData.date} is deleted." });
            else
                return BadRequest(new { message = "Error occured while deleting the Report." });
        }
    }
}