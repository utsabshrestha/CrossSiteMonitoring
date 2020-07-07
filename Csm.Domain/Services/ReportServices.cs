using Csm.Dto.Entities;
using CSM.Dal.Entities;
using CSM.Dal.Repositories;
using CSM.Dal.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Csm.Domain.Config;

namespace Csm.Domain.Services
{
    public class ReportServices : IReportServices
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<ReportServices> logger;

        public ReportServices(
            IUnitOfWork unitOfWork,
            ILogger<ReportServices> logger
            )
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<bool> changeReportStatus(string form_id, string roadCode, string observerEmail)
        {
            var status = 0;
            using (var uow = unitOfWork)
            {
                try
                {
                    uow.CreateConnection();
                    var initial = uow.InitialRepository();
                    status = await initial.UpdateInitialStatus(form_id, roadCode, observerEmail);
                    uow.CommitTransaction();
                }
                catch (Exception e)
                {
                    uow.RollbackTransaction();
                    logger.LogError(e, "Error occured (changeReportStatus): {msg}", e.Message);
                }
            }

            if (status == 0)
                return false;
            else
                return true;
        }

        public async Task<bool> UpdateConstructionObservation(ConstructionObservationDetail constructionObservationDetail)
        {
            ConstructionObservation observation = Mapping.Mapper.Map<ConstructionObservation>(constructionObservationDetail);
            var status = 0;
            using (var uow = unitOfWork)
            {
                try
                {
                    uow.CreateConnection();
                    var constructionRepo = uow.ConstructionObservationRepository();
                    status = await constructionRepo.UpdateObservation(observation);
                    uow.CommitTransaction();
                }
                catch (Exception e)
                {
                    uow.RollbackTransaction();
                    logger.LogError(e, "Error occured (UpdateConstructionObservation): {msg}", e.Message);
                }
            }
            if (status == 0)
                return false;
            else
                return true;
        }

        public async Task<bool> DeleteReport(string form_id, string road_code)
        {
            var status = false;
            using (var uow = unitOfWork)
            {
                try
                {
                    uow.CreateConnection();
                    await uow.InitialRepository().Delete(form_id, road_code);
                    await uow.ConstructionObservationRepository().DeleteObservation(form_id, road_code);
                    await uow.FilesRepository().Delete(form_id);
                    uow.CommitTransaction();
                    status = true;
                }
                catch (Exception e)
                {
                    uow.RollbackTransaction();
                    logger.LogError(e, "Error occured (DeleteReport): {msg}", e.Message);
                }
            }
            return status;
        }

        //TODO: Remove getreport from here - completed!.
    }
}