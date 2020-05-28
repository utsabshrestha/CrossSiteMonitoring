using Dapper;
using DataAccessLibrary.DataAccessLayer.Interfaces;
using Microsoft.Extensions.Configuration;
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
        public string GetConnectionString(string pathSqlite)
        {
            string Connection_string = "Data Source=" + pathSqlite + ";New=False;Compress=True;Synchronous=Off";
            return Connection_string;
        }

        public async Task<IEnumerable<T>> LoadSqLiteData<T, U>(string queries, U parameters, string PathSqlite)
        {
            string connectionString = GetConnectionString(PathSqlite);

            using (IDbConnection connection = new SQLiteConnection(connectionString))
            {
                return await connection.QueryAsync<T>(queries, parameters, commandType: CommandType.Text);
            }
        }
  
        public DataTable LoadSqLiteBlob(string queries, string PathSqlite)
        {
            string connectionString = GetConnectionString(PathSqlite);
            DataTable dataTable = new DataTable();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using SQLiteDataAdapter adapter = new SQLiteDataAdapter(queries, connection);
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        public void Dispose()
        {
           
        }
    }
}
