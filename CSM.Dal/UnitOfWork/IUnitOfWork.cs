using CSM.Dal.Repositories;
using System;

namespace CSM.Dal.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IConstructionObservationRepository GetConstructionObservationRepository { get; }
        IEventRepository GetEventRepository { get; }
        IFilesRepository GetFilesRepository { get; }
        IInitialRepository GetInitialRepository { get; }

        void CommitTransaction();
        IConstructionObservationRepository ConstructionObservationRepository();
        void CreateConnection();
        IEventRepository EventRepository();
        IFilesRepository FilesRepository();
        IInitialRepository InitialRepository();
        void RollbackTransaction();
    }
}