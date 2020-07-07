using Csm.Dto.Entities;
using CSM.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Csm.Domain.Services
{
    public interface IDashboard
    {
        string User { get; }
        Task<IEnumerable<DistrictDto>> getDistrict();
        Task<IEnumerable<RoadDto>> getRoads();
        Task<IEnumerable<RoadDto>> getRoads(string district);
        Task<IEnumerable<RoadDetailsDto>> GetRoadDetail(string roadCode, string districts);
    }
}
