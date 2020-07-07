using CSM.Dal.Entities;
using CSM.Dal.Helper;
using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace CSM.Dal.Internal
{
    public class DataAccess : IDataAccess
    {

        private CsmData settings;
        public DataAccess(IOptions<CsmData> options)
        {
            settings = options.Value;
        }

        public async Task<IEnumerable<T>> LoadData<T, U>(string queries, U parameters)
        {
            using (IDbConnection connection = new NpgsqlConnection(settings.Csmdb))
            {
                IEnumerable<T> rows = await connection.QueryAsync<T>(queries, parameters, commandType: CommandType.Text);
                return rows;
            }
        }

        //TODO make this generice instead - Completed!.
        public async Task<GenericReport<T, U, R>> getReport<T, U, R, K>(string sql, K parameters)
            where T : class where U : class where R : class
        {
            GenericReport<T, U, R> report = new GenericReport<T, U, R>();
            using (IDbConnection connection = new NpgsqlConnection(settings.Csmdb))
            {
                connection.Open();
                using (var multi = await connection.QueryMultipleAsync(sql, parameters))
                {
                    report.GetInitial = multi.Read<T>();
                    report.GetConstruction = multi.Read<U>();
                    report.GetFiles = multi.Read<R>();
                }
            }
            return report;
        }
    }
}