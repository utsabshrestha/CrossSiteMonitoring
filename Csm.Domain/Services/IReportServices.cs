using Csm.Dto.Entities;
using System.Threading.Tasks;

namespace Csm.Domain.Services
{
    public interface IReportServices
    {
        Task<bool> changeReportStatus(string form_id, string roadCode, string observerEmail);
        Task<bool> DeleteReport(string form_id, string road_code);
        Task<bool> UpdateConstructionObservation(ConstructionObservationDetail constructionObservationDetail);
    }
}