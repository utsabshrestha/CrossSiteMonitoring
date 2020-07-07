using CSM.Dal.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Csm.Domain.SynchronizeApi.Service
{
    public interface IDataInsertion
    {
        Task<bool> InsertDataToDatabase(Initial initials, IEnumerable<ConstructionObservation> observations, IEnumerable<Files> files, IEnumerable<EventRecording> events);
        Task<bool> UpdateDataToDatabase(Initial initials, IEnumerable<ConstructionObservation> observations, IEnumerable<Files> files, IEnumerable<EventRecording> eventRecordings);
    }
}