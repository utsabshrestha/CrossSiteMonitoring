using CSM.Dal.Entities;
using CSM.Dal.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace CSM.Dal.Repositories
{
    public class ReadSqlite : IReadSqlite
    {
        private readonly ISqliteDataAccess sqliteDataAccess;

        public ReadSqlite(ISqliteDataAccess sqliteDataAccess)
        {
            this.sqliteDataAccess = sqliteDataAccess;
        }

        public async Task<IEnumerable<Initial>> getInitial(string path)
        {
            string sql = @"select * from initial_details";
            return await sqliteDataAccess.LoadSqLiteData<Initial, dynamic>(sql, new { }, path);
        }

        public async Task<IEnumerable<ConstructionObservation>> getConstructionObservations(string path)
        {
            string sql = @"select * from construction_observation_detail";
            return await sqliteDataAccess.LoadSqLiteData<ConstructionObservation, dynamic>(sql, new { }, path);
        }

        public async Task<IEnumerable<Files>> getFiles(string path)
        {
            // not selecting blob columns.
            string sql = @"select uuid, file_id, form_id, file_name, file_note, unique_file, file_type from file";
            return await sqliteDataAccess.LoadSqLiteData<Files, dynamic>(sql, new { }, path);
        }

        public async Task<IEnumerable<EventRecording>> getEventRecordings(string path)
        {
            string sql = @"select * from event_recording";
            return await sqliteDataAccess.LoadSqLiteData<EventRecording, dynamic>(sql, new { }, path);
        }

        public DataTable getBlobImage(string path)
        {
            string sql = @"select file_name,blob_file from file";
            return sqliteDataAccess.LoadSqLiteBlob(sql, path);
        }
    }
}
