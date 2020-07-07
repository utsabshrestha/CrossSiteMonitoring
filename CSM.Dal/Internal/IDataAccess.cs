using CSM.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSM.Dal.Internal
{
    public interface IDataAccess
    {
        Task<GenericReport<T, U, R>> getReport<T,U,R,K>(string sql, K parameters)
            where T : class where U : class where R : class;

        Task<IEnumerable<T>> LoadData<T, U>(string queries, U parameters);
    }
}