using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Csm.Web.ViewModels
{
    public class FullReport
    {
        public FullReport()
        {
            inital = new List<Inital>();
            constructionObservations = new List<ConstructionObservation>();
            files = new List<Files>();
        }
        public IEnumerable<Inital> inital { get; set; }
        public IEnumerable<ConstructionObservation> constructionObservations { get; set; }
        public IEnumerable<Files> files { get; set; }
    }
}
