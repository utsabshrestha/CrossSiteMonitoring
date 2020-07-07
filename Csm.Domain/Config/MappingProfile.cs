using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AutoMapper;
using Csm.Dto.Entities;
using CSM.Dal.Entities;

namespace Csm.Domain.Config
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap<Source, Destination>();
            // Additional mappings here...
            CreateMap<ConstructionObservation, ConstructionObservationDetail>();
            CreateMap<ConstructionObservationDetail, ConstructionObservation>();
            CreateMap<Initial, InitialsDetails>();
            CreateMap<Files, FilesDetail>();
            CreateMap<Road, RoadDto>();
            CreateMap<RoadDetails, RoadDetailsDto>();
            CreateMap<District, DistrictDto>();
        }
    }
}