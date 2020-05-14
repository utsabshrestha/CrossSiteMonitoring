using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Csm.Services.ServiceInterface
{
    public interface IInventory
    {
        List<District> GetDistricts();
        List<RoadDetails> GetRoadDetails(string roadCode);
        List<RoadDetails> GetRoadDetails(string roadCode, string district);
        List<Road> GetRoads();
        List<Road> GetRoads(string district);
        List<ReportDataModel> GetReportDataList(string roadeCode, DateTime date, string observerEmail);
    }
}
