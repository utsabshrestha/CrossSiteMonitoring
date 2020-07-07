using CSM.Dal.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Csm.Domain.SynchronizeApi.Service
{
    public interface IReadSqliteData
    {
        Task<IEnumerable<ConstructionObservation>> GetConstructionObservations(string sqlitepath);
        Task<IEnumerable<EventRecording>> GetEventRecordings(string sqlitepath);
        Task<IEnumerable<Files>> GetFiles(string sqlitepath);
        Task<IEnumerable<Initial>> GetInitial(string sqlitepath);
    }
}