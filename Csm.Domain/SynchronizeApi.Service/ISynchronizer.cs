using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Csm.Domain.SynchronizeApi.Service
{
    public interface ISynchronizer
    {
        Task<bool> SynchronizeData();
    }
}
