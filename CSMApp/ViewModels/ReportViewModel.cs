using CSMApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMApp.ViewModels
{
    public class ReportViewModel
    {
        public ReportViewModel()
        {
            ReportViewDataModel = new List<ReportViewDataModel>();
        }
        public List<ReportViewDataModel> ReportViewDataModel { get; set; }
    }
}
