using Dapper;
using DataAccessLibrary.DataAccessLayer.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.DataAccessLayer.DataAccess
{
    public class SqlLiteDataAccess : IDisposable, ISqlLiteDataAccess
    {
        private readonly ILogger<SqlLiteDataAccess> logger;

        public SqlLiteDataAccess(ILogger<SqlLiteDataAccess> logger)
        {
            this.logger = logger;
        }

        public string GetConnectionString(string pathSqlite)
        {
            string Connection_string = $"Data Source={pathSqlite};New=False;Compress=True;Synchronous=Off";
            return Connection_string;
        }

        public async Task<IEnumerable<T>> LoadSqLiteData<T, U>(string queries, U parameters, string PathSqlite)
        {
            string connectionString = GetConnectionString(PathSqlite);
            try
            {
                using (IDbConnection connection = new SQLiteConnection(connectionString))
                {
                    return await connection.QueryAsync<T>(queries, parameters, commandType: CommandType.Text);
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                throw;
            }
        }
  
        public DataTable LoadSqLiteBlob(string queries, string PathSqlite)
        {
            string connectionString = GetConnectionString(PathSqlite);
            DataTable dataTable = new DataTable();
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    using SQLiteDataAdapter adapter = new SQLiteDataAdapter(queries, connection);
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                throw;
            }
        }

        public void Dispose()
        {
           
        }
    }
}
