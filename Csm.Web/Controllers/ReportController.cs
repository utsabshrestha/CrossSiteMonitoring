using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Csm.Domain.Config;
using Csm.Domain.Services;
using Csm.Web.Models;
using Csm.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Csm.Web.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly UserManager<ApplicationUserDomain> userManager;
        private readonly IReportQuery reportQuery;

        public ReportController(
            UserManager<ApplicationUserDomain> userManager,
            IReportQuery reportQuery
            )
        {
            this.userManager = userManager;
            this.reportQuery = reportQuery;
        }

        [HttpPost]
        public async Task<IActionResult> ViewReport(string form_id, string road_code, DateTime date, string observer_email, string redirect)
        {
            var fullReportDto = await reportQuery.getReport(form_id, road_code);

            FullReport fullReport = new FullReport
            {
                inital = fullReportDto.GetInitial,
                constructionObservations = fullReportDto.GetConstruction,
                files = fullReportDto.GetFiles
            };

            if (fullReportDto.GetInitial.Any())
            {
                ViewData["district"] = fullReportDto.GetInitial.ElementAt(0).district;
                ViewData["road_code"] = fullReportDto.GetInitial.ElementAt(0).road_code;
            }
            ViewData["Redirection"] = redirect;
            return View(fullReport);
        }

        [HttpPost]
        public async Task<IActionResult> EditConstructionReport(SelectedeReprotData selectedeReprotData, string redirect)
        {
            var applicationUser = await userManager.GetUserAsync(User);
            if (!User.IsInRole("Admin") && applicationUser?.Email != selectedeReprotData.observer_email)
                return RedirectToAction("AccessDenied", "Account");

            var fullReportDto = await reportQuery.getReport(selectedeReprotData.form_id, selectedeReprotData.road_code);

            FullReport fullReport = new FullReport
            {
                inital = fullReportDto.GetInitial,
                constructionObservations = fullReportDto.GetConstruction,
                files = fullReportDto.GetFiles
            };

            if (fullReportDto.GetInitial.Any())
            {
                ViewData["district"] = fullReportDto.GetInitial.ElementAt(0).district;
                ViewData["road_code"] = fullReportDto.GetInitial.ElementAt(0).road_code;
            }

            ViewData["message"] = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/ReportService/UpdateObservations";
            ViewData["Redirection"] = redirect;
            return View(fullReport);
        }

    }
}
