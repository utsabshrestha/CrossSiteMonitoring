using Csm.Dto.Entities;
using System.Collections.Generic;

namespace Csm.Web.ViewModels
{
    public class FullReport
    {
        public FullReport()
        {
            inital = new List<InitialsDetails>();
            constructionObservations = new List<ConstructionObservationDetail>();
            files = new List<FilesDetail>();
        }
        public IEnumerable<InitialsDetails> inital { get; set; }
        public IEnumerable<ConstructionObservationDetail> constructionObservations { get; set; }
        public IEnumerable<FilesDetail> files { get; set; }
    }
}