using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Csm.Services.ServiceInterface
{
    public interface ISyncApi
    {
        Task<SyncStatus> SyncData(SyncApiCred apiCred);
        void Inst();
    }
}
