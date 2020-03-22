using System.Collections.Generic;

namespace DataAccessLibrary.DataAccessLayer.DataAccess
{
    public interface ISqlDataAccess
    {
        void Dispose();
        string GetConnectionString(string ConnectionStringName);
        List<T> LoadData<T, U>(string queries, U parameters, string ConnectionStringName);
        List<T> LoadDataFrmSP<T, U>(string storedProcedure, U parameters, string connectionStringName);
    }
}