using System.Threading.Tasks;

namespace Csm.Domain.SynchronizeApi.Service
{
    public interface IImageExtractor
    {
        Task BlobImageWriter();
        Task GoogleLocationImageWriter();
    }
}