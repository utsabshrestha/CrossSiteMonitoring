using CSM.Dal.Entities;
using CSM.Dal.Repositories;
using CSM.Dal.UnitOfWork;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csm.Domain.SynchronizeApi.Service
{
    public class DataInsertion : IDataInsertion
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<DataInsertion> logger;

        public DataInsertion(IUnitOfWork unitOfWork, ILogger<DataInsertion> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<bool> InsertDataToDatabase(
            Initial initials,
            IEnumerable<ConstructionObservation> observations,
            IEnumerable<Files> files,
            IEnumerable<EventRecording> events
            )
        {
            var observationWithLine = observations.Where(observations => observations.location_type == "Line Location").ToList();
            var observationWithPoint = observations.Where(observations => observations.location_type == "Point Location").ToList();
            bool status = false;
            using (var uow = unitOfWork)
            {
                try
                {
                    uow.CreateConnection();
                    await uow.GetInitialRepository.Add(initials);
                    await uow.GetConstructionObservationRepository.AddLine(observationWithLine);
                    await uow.GetConstructionObservationRepository.AddPoint(observationWithPoint);
                    await uow.GetFilesRepository.Add(files);
                    await uow.GetEventRepository.Add(events);
                    uow.CommitTransaction();
                    status = true;
                }
                catch (Exception e)
                {
                    uow.RollbackTransaction();
                    logger.LogError(e, "Error occured while Syncronizing data : {message}", e.Message);
                }
            }
            return status;
        }

        public async Task<bool> UpdateDataToDatabase(
            Initial initials,
            IEnumerable<ConstructionObservation> observations,
            IEnumerable<Files> files,
            IEnumerable<EventRecording> eventRecordings
            )
        {
            var observationWithLine = observations.Where(observations => observations.location_type == "Line Location").ToList();
            var observationWithPoint = observations.Where(observations => observations.location_type == "Point Location").ToList();
            bool status = false;

            using (var uow = unitOfWork)
            {
                try
                {
                    uow.CreateConnection();
                    await uow.GetInitialRepository.Update(initials);
                    await uow.GetConstructionObservationRepository.UpdateLine(observationWithLine);
                    await uow.GetConstructionObservationRepository.UpdatePoint(observationWithPoint);
                    await uow.GetEventRepository.Update(eventRecordings);
                    uow.CommitTransaction();
                    status = true;
                }
                catch (Exception e)
                {
                    uow.RollbackTransaction();
                    logger.LogError(e, "Error occured while Syncronizing data : {message}", e.Message);
                }
            }
            return status;
        }
    }
}