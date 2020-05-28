using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary.DataAccess
{
    public interface IInventory
    {
        Task<IEnumerable<District>> GetDistricts();
        Task<IEnumerable<RoadDetails>> GetRoadDetails(string roadCode);
        Task<IEnumerable<RoadDetails>> GetRoadDetails(string roadCode, string district);
        Task<IEnumerable<Road>> GetRoads();
        Task<IEnumerable<Road>> GetRoads(string district);
        Task<IEnumerable<ReportDataModel>> GetReportDataList(string roadeCode, DateTime date, string observerEmail);
    }
}