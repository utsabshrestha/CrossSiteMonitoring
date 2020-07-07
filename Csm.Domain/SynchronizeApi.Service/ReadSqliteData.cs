using CSM.Dal.Entities;
using CSM.Dal.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Csm.Domain.SynchronizeApi.Service
{
    public class ReadSqliteData : IReadSqliteData
    {
        private readonly IReadSqlite readSqlite;

        public ReadSqliteData(IReadSqlite readSqlite)
        {
            this.readSqlite = readSqlite;
        }

        public async Task<IEnumerable<Initial>> GetInitial(string sqlitepath)
        {
            return await readSqlite.getInitial(sqlitepath);
        }

        public async Task<IEnumerable<ConstructionObservation>> GetConstructionObservations(string sqlitepath)
        {
            var constructionObservations = await readSqlite.getConstructionObservations(sqlitepath);

            foreach (var observation in constructionObservations)
            {
                observation.construction_type.Replace('\'', ' ');
                observation.observation_notes.Replace('\'', ' ');
                observation.location.Replace('\'', ' ');

                if (observation.location_type == "Line Location")
                {
                    if (observation.line_latitude_to < 26 || observation.line_latitude_to > 31)
                        observation.line_latitude_to = observation.line_latitude_from;

                    if (observation.line_longitude_to < 81 || observation.line_longitude_to > 89)
                        observation.line_longitude_to = observation.line_longitude_from;
                }
            }
            return constructionObservations;
        }

        public async Task<IEnumerable<Files>> GetFiles(string sqlitepath)
        {
            return await readSqlite.getFiles(sqlitepath);
        }

        public async Task<IEnumerable<EventRecording>> GetEventRecordings(string sqlitepath)
        {
            return await readSqlite.getEventRecordings(sqlitepath);
        }
    }
}
