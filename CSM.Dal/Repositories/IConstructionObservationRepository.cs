using CSM.Dal.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSM.Dal.Repositories
{
    public interface IConstructionObservationRepository
    {
        Task AddLine(IEnumerable<ConstructionObservation> constructionObservation);
        Task AddPoint(IEnumerable<ConstructionObservation> constructionObservation);
        Task DeleteObservation(string uuid, string road_code);
        Task UpdateLine(IEnumerable<ConstructionObservation> constructionObservation);
        Task<int> UpdateObservation(ConstructionObservation constructionObservation);
        Task UpdatePoint(IEnumerable<ConstructionObservation> constructionObservation);
    }
}