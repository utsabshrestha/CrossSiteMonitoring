using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Csm.Web.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            var queryString = statusCodeResult?.OriginalQueryString;
            var path = statusCodeResult?.OriginalPath;
            if(statusCodeResult != null)
            {
                switch (statusCode)
                {
                    case 404:
                        ViewBag.ErrorMessage = "Sorry, the resource you requested could not be found.";
                        logger.LogWarning("404 Error Occured. Path = {Path} and QueryString= {QueryString}", path, queryString);
                        break;
                }
            }
            return View("NotFound");
        }

        [HttpGet]
        [Route("Error")]
        [AllowAnonymous]
        public IActionResult Error()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if(exceptionDetails != null)
            {
                //logger.LogError("The path {Path} threw an Exception {Error}",exceptionDetails.Path, exceptionDetails.Error);
                logger.LogError(exceptionDetails.Error, "The path {Path} threw an Exception: {msg}", exceptionDetails.Path, exceptionDetails.Error.Message);
                ViewBag.ErrorMessage = @"An error occurred while processing your request.The support team is notified and we are working on the fix";
            }
            return View("Error");
        }
    }
}
