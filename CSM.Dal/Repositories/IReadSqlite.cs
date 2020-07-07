using CSM.Dal.Entities;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace CSM.Dal.Repositories
{
    public interface IReadSqlite
    {
        Task<IEnumerable<ConstructionObservation>> getConstructionObservations(string path);
        Task<IEnumerable<EventRecording>> getEventRecordings(string path);
        Task<IEnumerable<Files>> getFiles(string path);
        Task<IEnumerable<Initial>> getInitial(string path);
        DataTable getBlobImage(string path);
    }
}