using CSM.Dal.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSM.Dal.Repositories
{
    public interface IEventRepository
    {
        Task Add(IEnumerable<EventRecording> eventRecording);
        Task Update(IEnumerable<EventRecording> eventRecording);
    }
}