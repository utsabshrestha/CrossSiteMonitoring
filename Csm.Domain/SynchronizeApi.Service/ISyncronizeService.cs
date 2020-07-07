using Csm.Dto.Entities;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Csm.Domain.SynchronizeApi.Service
{
    public interface ISyncronizeService
    {
        Task<bool> Syncronize(SyncApiCred syncApiCred);
    }
}