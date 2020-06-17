using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.DataAccessLayer.Interfaces
{
    public interface ISqlDataAccess
    {
        void Dispose();
        string GetConnectionString(string ConnectionStringName);
        Task<IEnumerable<T>> LoadData<T, U>(string queries, U parameters, string ConnectionStringName);
        IList<T> LoadDataFrmSP<T, U>(string storedProcedure, U parameters, string connectionStringName);
        void InsertSp<U>(string sp, List<U> param, string cons);
        Task<int> InsertRow<U>(string query, U parameters, string connectionStringName);
        void StartTransaction(string ConnectionStringName);
        Task SaveDataInTransaction<U>(string queries, List<U> parameters);
        Task SaveDataInTransaction<U>(string queries, U parameters);

        void CommintTransaction();

        void RollbackTransaction();
    }
}
