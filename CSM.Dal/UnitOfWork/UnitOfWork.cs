using CSM.Dal.Helper;
using CSM.Dal.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CSM.Dal.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbConnection connection;
        private IDbTransaction transaction;
        private IInitialRepository initialRepository;
        private IConstructionObservationRepository constructionObservationRepository;
        private IFilesRepository filesRepository;
        private IEventRepository eventRepository;
        private CsmData settings;

        public UnitOfWork(IOptions<CsmData> options)
        {
            settings = options.Value;
        }

        public void CreateConnection()
        {
            if (connection == null)
            {
                connection = new NpgsqlConnection(settings.Csmdb);
                connection?.Open();
                transaction = connection?.BeginTransaction();
            }
        }

        public IInitialRepository InitialRepository()
        {
            if (connection == null)
            {
                CreateConnection();
            }
            return initialRepository ?? (initialRepository = new InitialRepository(transaction));
        }

        public IInitialRepository GetInitialRepository
        {
            get
            {
                if (connection == null)
                {
                    CreateConnection();
                }
                return initialRepository ?? (initialRepository = new InitialRepository(transaction));
            }
        }

        public IConstructionObservationRepository ConstructionObservationRepository()
        {

            if (connection == null)
            {
                CreateConnection();
            }
            return constructionObservationRepository ?? (constructionObservationRepository = new ConstructionObservationRepository(transaction));
        }


        public IConstructionObservationRepository GetConstructionObservationRepository
        {
            get
            {
                if (connection == null)
                {
                    CreateConnection();
                }
                return constructionObservationRepository ?? (constructionObservationRepository = new ConstructionObservationRepository(transaction));
            }
        }



        public IFilesRepository FilesRepository()
        {
            if (connection == null)
            {
                CreateConnection();
            }
            return filesRepository ?? (filesRepository = new FilesRepository(transaction));

        }


        public IFilesRepository GetFilesRepository
        {
            get
            {
                if (connection == null)
                {
                    CreateConnection();
                }
                return filesRepository ?? (filesRepository = new FilesRepository(transaction));
            }
        }


        public IEventRepository EventRepository()
        {
            if (connection == null)
            {
                CreateConnection();
            }
            return eventRepository ?? (eventRepository = new EventRepository(transaction));
        }

        public IEventRepository GetEventRepository
        {
            get
            {
                if (connection == null)
                {
                    CreateConnection();
                }
                return eventRepository ?? (eventRepository = new EventRepository(transaction));
            }
        }

        public void CommitTransaction()
        {
            transaction?.Commit();
            connection?.Close();
            Reset();
            Dispose();
        }

        public void RollbackTransaction()
        {
            transaction?.Rollback();
            connection?.Close();
            Reset();
            Dispose();
        }

        public void Reset()
        {
            initialRepository = null;
            constructionObservationRepository = null;
            filesRepository = null;
            eventRepository = null;
        }

        public void Dispose()
        {
            if (transaction != null)
            {
                transaction.Dispose();
            }
            transaction = null;

            if (connection != null)
            {
                connection.Dispose();
            }
            connection = null;
        }
    }
}