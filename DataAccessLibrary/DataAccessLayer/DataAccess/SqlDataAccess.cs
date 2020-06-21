using Dapper;
using DataAccessLibrary.DataAccessLayer.Interfaces;
using DataAccessLibrary.DataHelper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.DataAccessLayer.DataAccess
{
    public class SqlDataAccess : IDisposable, ISqlDataAccess
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private bool isClosed = false;
        private CsmData settings;
        private readonly ILogger<SqlDataAccess> logger;

        public SqlDataAccess(IOptions<CsmData> options, ILogger<SqlDataAccess> logger)
        {
            settings = options.Value;
            this.logger = logger;
        }

        public string GetConnectionString(String ConnectionStringName)
        {
            return settings.Csmdb;
        }

        public async Task<IEnumerable<T>> LoadData<T, U>(string queries, U parameters, string ConnectionStringName)
        {
            string ConnectionString = GetConnectionString(ConnectionStringName);

            using (IDbConnection connection = new NpgsqlConnection(ConnectionString))
            {
                IEnumerable<T> rows = await connection.QueryAsync<T>(queries, parameters, commandType: CommandType.Text);
                return rows;
            }
        }


        public IList<T> LoadDataFrmSP<T, U>(string storedProcedure, U parameters, string connectionStringName)
        {
            string ConnectionString = GetConnectionString(connectionStringName);

            using (IDbConnection connection = new NpgsqlConnection(ConnectionString))
            {
                List<T> rows = connection.Query<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure).ToList();
                return rows;
            }
        }

        public async Task<int> ExecuteRow<U>(string query, U parameters, string connectionStringName)
        {
            string ConnectionString = GetConnectionString(connectionStringName);

            using (IDbConnection connection = new NpgsqlConnection(ConnectionString))
            {
                return await connection.ExecuteAsync(query, parameters, commandType: CommandType.Text);
            }
        }

        public void InsertSp<U>(string sp, List<U> param,string cons)
        {
            string ConnectionString = GetConnectionString(cons);

            using (IDbConnection connection = new NpgsqlConnection(ConnectionString))
            {
                var rows = connection.Execute(sp, param, commandType: CommandType.Text);
                Console.WriteLine(rows);
            }
        }

        public void StartTransaction(string ConnectionStringName)
        {
            string connectionString = GetConnectionString(ConnectionStringName);

            _connection = new NpgsqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
            isClosed = false;
        }

        public async Task SaveDataInTransaction<U>(string queries, List<U> parameters)
        {
            await _connection.ExecuteAsync(queries, parameters, commandType: CommandType.Text, transaction: _transaction);
        }

        public async Task SaveDataInTransaction<U>(string queries, U parameters)
        {
            await _connection.ExecuteAsync(queries, parameters, commandType: CommandType.Text, transaction: _transaction);
        }

        public void CommintTransaction()
        {
            _transaction?.Commit();
            _connection?.Close();

            isClosed = true;
        }

        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _connection?.Close();

            isClosed = true;
        }

        public void Dispose()
        {
            if (!isClosed)
            {
                try
                {
                    CommintTransaction();
                }
                catch (Exception e)
                {
                    // TODO: Log this issue
                    logger.LogError(e, "Error occured with Message {message}", e.Message);
                }
            }

            _transaction = null;
            _connection = null;
        }
    }
}
