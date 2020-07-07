using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Csm.Domain.SynchronizeApi.Service
{
    public interface ICreateFile
    {
        Task<bool> WriteFile(IFormFile formFile);
    }
}