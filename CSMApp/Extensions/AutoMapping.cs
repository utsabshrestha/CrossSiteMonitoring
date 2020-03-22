using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CSMApp.Models;
using CSMApp.ViewModels;
using DataAccessLibrary.Models;

namespace CSMApp.Extensions
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Road, RoadList>();
            CreateMap<RoadDetails, DetailRoadList>();
            CreateMap<ReportDataModel, ReportViewDataModel>();
        }
    }
}
