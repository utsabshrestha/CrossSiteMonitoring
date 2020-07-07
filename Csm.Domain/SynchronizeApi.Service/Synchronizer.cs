using Csm.Dto.Entities;
using CSM.Dal.Entities;
using CSM.Dal.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csm.Domain.SynchronizeApi.Service
{
    public class Synchronizer : ISynchronizer
    {
        private readonly IDataInsertion dataInsertion;
        private readonly IMonitoringRepository monitoringRepository;
        private readonly IReadSqlite readSqlite;
        private readonly ISqlitePath sqlitePath;

        public Synchronizer(
            IDataInsertion dataInsertion, 
            IMonitoringRepository monitoringRepository,
            IReadSqlite readSqlite,
            ISqlitePath sqlitePath
            )
        {
            this.dataInsertion = dataInsertion;
            this.monitoringRepository = monitoringRepository;
            this.readSqlite = readSqlite;
            this.sqlitePath = sqlitePath;
        }

        public async Task<bool> SynchronizeData()
        {
            IEnumerable<Initial> initials = await readSqlite.getInitial(sqlitePath.path);
            IEnumerable<ConstructionObservation> constructionObservations = await readSqlite.getConstructionObservations(sqlitePath.path);
            IEnumerable<Files> files = await readSqlite.getFiles(sqlitePath.path);
            IEnumerable<EventRecording> eventRecordings = await readSqlite.getEventRecordings(sqlitePath.path);
            bool status = true;
            foreach (var initial in initials)
            {
                var observation = constructionObservations.Where(x => x.uuid == initial.form_id);
                var obsFiles = files.Where(x => x.uuid == initial.form_id);
                var events = eventRecordings.Where(x => x.uuid == initial.form_id);
                bool insertStatus = false;

                if ( await ReportExistence(initial))
                {
                    insertStatus = await dataInsertion.UpdateDataToDatabase(initial,observation, obsFiles, events);
                }
                else
                {
                    insertStatus = await dataInsertion.InsertDataToDatabase(initial, observation, obsFiles, events);
                }

                if (!insertStatus)
                {
                    status = false;
                }
            }
            return status;
        }

        private async Task<bool> ReportExistence(Initial initial)
        {
            var report = await monitoringRepository.CheckReport(initial.form_id, initial.road_code, initial.observer_email);

            if(report.Count() == 0)
                return false;
            else
                return true;
        }
    }
}
