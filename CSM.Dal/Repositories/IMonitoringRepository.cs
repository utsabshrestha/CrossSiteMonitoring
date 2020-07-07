using CSM.Dal.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSM.Dal.Repositories
{
    public interface IMonitoringRepository
    {
        Task<IEnumerable<Initial>> CheckReport(string form_id, string roadeCode, string observerEmail);
        Task<IEnumerable<District>> GetDistricts();
        Task<IEnumerable<RoadDetails>> GetRoadDetails(string roadCode);
        Task<IEnumerable<RoadDetails>> GetRoadDetails(string roadCode, string district);
        Task<IEnumerable<Road>> GetRoads();
        Task<IEnumerable<Road>> GetRoads(string district);
        Task<IEnumerable<District>> GetUserSpecificDistircts(string observer_email);
        Task<IEnumerable<Road>> GetUserSpecificRoads(string observer_email);
        Task<IEnumerable<Road>> GetUserSpecificRoads(string observer_email, string district);
        Task<GenericReport<T, U, R>> GetWholeReport<T, U, R>(string form_id, string road_code)
            where T : class where U : class where R : class ;
    }
}