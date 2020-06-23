using Csm.Web.Models;
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
            inital = new List<InitialsDetails>();
            constructionObservations = new List<ConstructionObservationDetail>();
            files = new List<FilesDetails>();
        }
        public IEnumerable<InitialsDetails> inital { get; set; }
        public IEnumerable<ConstructionObservationDetail> constructionObservations { get; set; }
        public IEnumerable<FilesDetails> files { get; set; }
    }
}