using CSM.Dal.Entities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace CSM.Dal.Repositories
{
    internal class ConstructionObservationRepository : RepositoryBase, IConstructionObservationRepository
    {
        public ConstructionObservationRepository(IDbTransaction transaction) : base(transaction)
        {

        }

        public async Task AddPoint(IEnumerable<ConstructionObservation> constructionObservation)
        {
            string sql = @"INSERT INTO monitoring.construction_observation_detail 
                        (uuid, form_id, construction_type, location, observation_notes, quality_rating, latitude,longitude, altitude,date, road_code,location_type, 
                        point_geom) VALUES 
                        (@uuid, @form_id, @construction_type, @location, @observation_notes, @quality_rating, @latitude,@longitude,@altitude,@date,@road_code,@location_type,
                        ST_SetSRID(ST_MakePoint(@longitude,@latitude),4326))";

            await Connection.ExecuteScalarAsync<string>(sql, constructionObservation, transaction: Transaction);
        }

        public async Task AddLine(IEnumerable<ConstructionObservation> constructionObservation)
        {
            string sql = @"INSERT INTO monitoring.construction_observation_detail 
                        (uuid, form_id, construction_type, location, observation_notes, quality_rating, altitude,date, road_code,location_type, 
                        the_geom,line_latitude_from,line_longitude_from,line_latitude_to,line_longitude_to) VALUES 
                        (@uuid, @form_id, @construction_type, @location, @observation_notes, @quality_rating, @altitude,@date,@road_code,@location_type,
                        ST_SetSRID(ST_MakeLine(ST_MakePoint(@line_longitude_from,@line_latitude_from), ST_MakePoint(@line_longitude_to,@line_latitude_to)),4326),
                        @line_latitude_from,@line_longitude_from,@line_latitude_to,@line_longitude_to)";

            await Connection.ExecuteAsync(sql, constructionObservation, transaction: Transaction);
        }

        public async Task UpdatePoint(IEnumerable<ConstructionObservation> constructionObservation)
        {
            string sql = @"UPDATE monitoring.construction_observation_detail SET road_code=@road_code, construction_type=@construction_type,
                        location=@location, observation_notes=@observation_notes, quality_rating=@quality_rating, latitude=@latitude, longitude=@longitude, 
                        line_latitude_from= @line_latitude_from, line_longitude_from=@line_longitude_from, line_latitude_to=@line_latitude_to, 
                        line_longitude_to=@line_longitude_to, altitude=@altitude, date=@date , 
                        point_geom= ST_SetSRID(ST_MakePoint(@longitude,@latitude),4326) where form_id= @form_id";

            await Connection.ExecuteAsync(sql, constructionObservation, transaction: Transaction);
        }

        public async Task UpdateLine(IEnumerable<ConstructionObservation> constructionObservation)
        {
            string sql = @"UPDATE monitoring.construction_observation_detail SET road_code=@road_code, construction_type=@construction_type,
                        location=@location, observation_notes=@observation_notes, quality_rating=@quality_rating, latitude=@latitude, longitude=@longitude, 
                        line_latitude_from= @line_latitude_from, line_longitude_from=@line_longitude_from, line_latitude_to=@line_latitude_to, line_longitude_to=@line_longitude_to, 
                        altitude=@altitude, date=@date ,
                        the_geom = ST_SetSRID(ST_MakeLine(ST_MakePoint(@line_longitude_from,@line_latitude_from), ST_MakePoint(@line_longitude_to,@line_latitude_to)),4326)
                        where form_id= @form_id";

            await Connection.ExecuteAsync(sql, constructionObservation, transaction: Transaction);
        }

        public async Task<int> UpdateObservation(ConstructionObservation constructionObservation)
        {
            string sql = @"update monitoring.construction_observation_detail set construction_type = @ConsType,
                    location=@LocAtion, observation_notes = @ObservNotes, quality_rating = @QualityRating where form_id = @FormId";

            var parameters = new
            {
                ConsType = constructionObservation.construction_type,
                LocAtion = constructionObservation.location,
                ObservNotes = constructionObservation.observation_notes,
                QualityRating = constructionObservation.quality_rating,
                FormId = constructionObservation.form_id
            };

            return await Connection.ExecuteAsync(sql, parameters, transaction: Transaction);
        }

        public async Task DeleteObservation(string uuid, string road_code)
        {
            string sql = @"delete from monitoring.construction_observation_detail where uuid = @UUid and road_code = @RoadCode";

            await Connection.ExecuteAsync(sql, new { UUid = uuid, RoadCode = road_code }, transaction: Transaction);
        }

    }
}