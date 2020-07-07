using CSM.Dal.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSM.Dal.Repositories
{
    public interface IFilesRepository
    {
        Task Add(IEnumerable<Files> files);
        Task Delete(string form_id);
    }
}