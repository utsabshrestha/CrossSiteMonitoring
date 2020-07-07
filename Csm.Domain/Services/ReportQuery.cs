using Csm.Domain.Config;
using Csm.Dto.Entities;
using CSM.Dal.Entities;
using CSM.Dal.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Csm.Domain.Services
{
    public class ReportQuery : IReportQuery
    {
        private readonly IMonitoringRepository monitoringRepository;

        public ReportQuery(
            IMonitoringRepository monitoringRepository
            )
        {
            this.monitoringRepository = monitoringRepository;
        }

        public async Task<FullReportDto<InitialsDetails, ConstructionObservationDetail, FilesDetail>> getReport(string form_id, string road_code)
        {
            var report = await monitoringRepository.GetWholeReport<Initial, ConstructionObservation,Files>(form_id, road_code);

            FullReportDto<InitialsDetails, ConstructionObservationDetail, FilesDetail> fullReport = new FullReportDto<InitialsDetails, ConstructionObservationDetail, FilesDetail>();
            fullReport.GetInitial = Mapping.Mapper.Map<IEnumerable<InitialsDetails>>(report.GetInitial);
            fullReport.GetConstruction = Mapping.Mapper.Map<IEnumerable<ConstructionObservationDetail>>(report.GetConstruction);
            fullReport.GetFiles = Mapping.Mapper.Map<IEnumerable<FilesDetail>>(report.GetFiles);
            return fullReport;
        }
    }
}
