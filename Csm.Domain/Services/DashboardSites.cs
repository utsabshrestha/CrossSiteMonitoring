using AutoMapper;
using Csm.Domain.Config;
using Csm.Dto.Entities;
using CSM.Dal.Entities;
using CSM.Dal.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Csm.Domain.Services
{
    public class DashboardSites : IDashboard
    {
        private readonly IMonitoringRepository monitoringRepository;

        public DashboardSites(IMonitoringRepository monitoringRepository)
        {
            this.monitoringRepository = monitoringRepository;
        }

        public string User => "sites";

        public async Task<IEnumerable<DistrictDto>> getDistrict()
        {
            var districts = await monitoringRepository.GetDistricts();
            var districtDto = Mapping.Mapper.Map<IEnumerable<DistrictDto>>(districts);
            return districtDto;
        }

        public async Task<IEnumerable<RoadDetailsDto>> GetRoadDetail(string roadCode, string districts)
        {
            if (districts == "none")
                return await getRoadDetails(roadCode);
            else
                return await getRoadDetails(roadCode, districts);
        }

        private async Task<IEnumerable<RoadDetailsDto>> getRoadDetails(string roadCode)
        {
            var roadDetails = await monitoringRepository.GetRoadDetails(roadCode);
            var roadDetailsDto = Mapping.Mapper.Map<IEnumerable<RoadDetailsDto>>(roadDetails);
            return roadDetailsDto;
        }

        private async Task<IEnumerable<RoadDetailsDto>> getRoadDetails(string roadCode, string districts)
        {
            var roadDetails = await monitoringRepository.GetRoadDetails(roadCode, districts);
            var roadDetailsDto = Mapping.Mapper.Map<IEnumerable<RoadDetailsDto>>(roadDetails);
            return roadDetailsDto;
        }

        public async Task<IEnumerable<RoadDto>> getRoads()
        {
            var roads = await monitoringRepository.GetRoads();
            var roadDto = Mapping.Mapper.Map<IEnumerable<RoadDto>>(roads);
            return roadDto;
        }

        public async Task<IEnumerable<RoadDto>> getRoads(string district)
        {
            var roads = await monitoringRepository.GetRoads(district);
            var roadDto = Mapping.Mapper.Map<IEnumerable<RoadDto>>(roads);
            return roadDto;
        }
    }
}
