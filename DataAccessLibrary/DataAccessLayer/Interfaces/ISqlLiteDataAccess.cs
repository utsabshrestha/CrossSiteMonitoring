using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.DataAccessLayer.Interfaces
{
    public interface ISqlLiteDataAccess
    {
        string GetConnectionString(string ConnectionStringName);

        Task<IEnumerable<T>> LoadSqLiteData<T, U>(string queries, U parameter, string PathSqlite);

        DataTable LoadSqLiteBlob(string queries, string PathSqlite);
    }
}
