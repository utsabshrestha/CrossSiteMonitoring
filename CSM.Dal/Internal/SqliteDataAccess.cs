using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace CSM.Dal.Internal
{
    public class SqliteDataAccess : ISqliteDataAccess
    {

        private string GetConnectionString(string pathSqlite)
        {
            string Connection_string = $"Data Source={pathSqlite};New=False;Compress=True;Synchronous=Off";
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
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(queries, connection))
                {
                    adapter.Fill(dataTable);
                }
                connection.Close();
            }
            return dataTable;
        }
    }
}