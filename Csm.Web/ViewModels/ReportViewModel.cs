using Csm.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Csm.Web.ViewModels
{
    public class ReportViewModel
    {
        public ReportViewModel()
        {
            ReportData = new List<ReportModel>();
        }
        public List<ReportModel> ReportData { get; set; }
    }
}
