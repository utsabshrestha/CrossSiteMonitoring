using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DataAccessLibrary.DataAccessLayer.DataAccess
{
    public class SqlDataAccess : IDisposable, ISqlDataAccess
    {
        private readonly IConfiguration configuration;

        public SqlDataAccess(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GetConnectionString(String ConnectionStringName)
        {
            return configuration.GetConnectionString(ConnectionStringName);
        }

        public List<T> LoadData<T, U>(string queries, U parameters, string ConnectionStringName)
        {
            string ConnectionString = GetConnectionString(ConnectionStringName);

            using (IDbConnection connection = new NpgsqlConnection(ConnectionString))
            {
                List<T> rows = connection.Query<T>(queries, parameters, commandType: CommandType.Text).ToList();
                return rows;
            }
        }


        public List<T> LoadDataFrmSP<T, U>(string storedProcedure, U parameters, string connectionStringName)
        {
            string ConnectionString = GetConnectionString(connectionStringName);

            using (IDbConnection connection = new NpgsqlConnection(ConnectionString))
            {
                List<T> rows = connection.Query<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure).ToList();
                return rows;
            }
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
