using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Csm.Services.ServiceInterface
{
    public interface IInventory
    {
        Task<IEnumerable<District>> GetDistricts();
        Task<IEnumerable<RoadDetails>> GetRoadDetails(string roadCode);
        Task<IEnumerable<RoadDetails>> GetRoadDetails(string roadCode, string district);
        Task<IEnumerable<Road>> GetRoads();
        Task<IEnumerable<Road>> GetRoads(string district);
        Task<IEnumerable<ReportDataModel>> GetReportDataList(string form_id ,string roadeCode, string observerEmail);
        Task<IEnumerable<Inital>> CheckReportEmail(string form_id, string roadeCode, string observerEmail);
        Task<int> UpdateReportStatus(string form_id, string roadCode, string observerEmail);
        Task<GenericReport<T, U, R>> GetWholeReport<T, U, R>(string form_id, string road_code);
        Task<int> UpdateConstructionObservation(ConstructionObservation constructionObservation);
        Task<bool> DeleteReportObservation(string form_id, string road_code);
    }
}
        
