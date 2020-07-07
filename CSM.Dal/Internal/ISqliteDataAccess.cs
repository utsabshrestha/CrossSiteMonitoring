using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace CSM.Dal.Internal
{
    public interface ISqliteDataAccess
    {
        DataTable LoadSqLiteBlob(string queries, string PathSqlite);
        Task<IEnumerable<T>> LoadSqLiteData<T, U>(string queries, U parameters, string PathSqlite);
    }
}