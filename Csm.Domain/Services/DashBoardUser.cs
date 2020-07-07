using AutoMapper;
using Csm.Dto.Entities;
using CSM.Dal.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Csm.Domain.Config;
using Microsoft.AspNetCore.Identity;

namespace Csm.Domain.Services
{
    public class DashBoardUser : IDashboard
    {
        private readonly IMonitoringRepository monitoringRepository;
        private readonly IHttpContextAccessor accessor;
        private readonly UserManager<ApplicationUserDomain> userManager;

        //TODO : ADD services.AddHttpContextAccessor() Completed;

        public DashBoardUser(
            IMonitoringRepository monitoringRepository,
            IHttpContextAccessor accessor,
            UserManager<ApplicationUserDomain> userManager
            )
        {
            this.monitoringRepository = monitoringRepository;
            this.accessor = accessor;
            this.userManager = userManager;
        }
        
        public string User => "mysites";

        private Task<ApplicationUserDomain> getLoggedInUser() => userManager.GetUserAsync(accessor.HttpContext.User);

        public async Task<IEnumerable<DistrictDto>> getDistrict()
        {
            var user = await getLoggedInUser();
            var district = await monitoringRepository.GetUserSpecificDistircts(user.Email);
            var districtDto = Mapping.Mapper.Map<IEnumerable<DistrictDto>>(district);
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
            var user = await getLoggedInUser();
            var road = await monitoringRepository.GetUserSpecificRoads(user.Email);
            var roadDto = Mapping.Mapper.Map<IEnumerable<RoadDto>>(road);
            return roadDto;
        }

        public async Task<IEnumerable<RoadDto>> getRoads(string district)
        {
            var user = await getLoggedInUser();
            var road = await monitoringRepository.GetUserSpecificRoads(user.Email, district);
            var roadDto = Mapping.Mapper.Map<IEnumerable<RoadDto>>(road);
            return roadDto;
        }
    }
}