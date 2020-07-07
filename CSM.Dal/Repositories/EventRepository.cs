using CSM.Dal.Entities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace CSM.Dal.Repositories
{
    internal class EventRepository : RepositoryBase, IEventRepository
    {
        public EventRepository(IDbTransaction transaction) : base(transaction)
        {

        }

        public async Task Add(IEnumerable<EventRecording> eventRecording)
        {
            string sql = @"insert into monitoring.event_recording
                        (uuid,district,road_link ,river_name,latitude,longitude,division,events,remarks,observations, msg_priority,form_id, date, road_code) 
                        values ( @uuid, @district, @road_link, @river_name, @latitude, @longitude, @division, @events, @remarks,
                        @observations, @msg_priority, @form_id, @date, @road_code)";

            await Connection.ExecuteScalarAsync<string>(sql, eventRecording, transaction: Transaction);
        }

        public async Task Update(IEnumerable<EventRecording> eventRecording)
        {
            string sql = @"UPDATE monitoring.event_recording SET
                        district = @district,road_link = @road_link,river_name = @river_name,latitude = @latitude,longitude = @longitude,division = @division,events = @events,
                        remarks = @remarks,observations = @observations, msg_priority = @msg_priority,form_id =  @form_id, date = @date, road_code = @road_code
                        WHERE form_id=@form_id";

            await Connection.ExecuteAsync(sql, eventRecording, transaction: Transaction);
        }
    }
}
