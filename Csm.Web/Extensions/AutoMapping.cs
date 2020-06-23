using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Csm.Web.Models;
using Csm.Web.ViewModels;
using DataAccessLibrary.Models;

namespace Csm.Web.Extensions
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Road, RoadList>();
            CreateMap<RoadDetails, DetailRoadList>();
            CreateMap<ReportDataModel, ReportModel>();
            CreateMap<Inital, InitialsDetails>();
            CreateMap<ConstructionObservation, ConstructionObservationDetail>();
            CreateMap<Files, FilesDetails>();
        }
    }
}
