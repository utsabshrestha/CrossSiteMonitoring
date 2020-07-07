using Csm.Dto.Entities;
using System.Threading.Tasks;

namespace Csm.Domain.Services
{
    public interface IReportQuery
    {
        Task<FullReportDto<InitialsDetails, ConstructionObservationDetail, FilesDetail>> getReport(string form_id, string road_code);
    }
}