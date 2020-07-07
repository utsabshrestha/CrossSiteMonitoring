using CSM.Dal.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSM.Dal.Repositories
{
    public interface IInitialRepository
    {
        Task Add(Initial initial);
        Task Delete(string form_id, string road_code);
        Task Update(Initial initial);
        Task<int> UpdateInitialStatus(string form_id, string roadCode, string observerEmail);
    }
}