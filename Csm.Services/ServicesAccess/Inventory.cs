using Csm.Services.ServiceInterface;
using DataAccessLibrary.DataAccessLayer.Interfaces;
using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Csm.Services.ServicesAccess
{
    public class Inventory : IInventory
    {
        private readonly ISqlDataAccess sqlDataAccess;

        public Inventory(ISqlDataAccess sqlDataAccess)
        {
            this.sqlDataAccess = sqlDataAccess;
        }

        public async Task<IEnumerable<District>> GetDistricts()
        {
            string query = @"select id, district_name, dcode from monitoring.tbl_district order by district_name asc;";

            var output = await sqlDataAccess.LoadData<District, dynamic>(query, new { }, "Csmdb");

            return output;
        }

        public async Task<IEnumerable<Road>> GetRoads()
        {
            string query = @"select id.road_code, id.district, max(id.uploaded_date) as last_uploaded_date 
                            from monitoring.initial_details id
                            join (select uuid from monitoring.construction_observation_detail group by uuid) c on c.uuid = id.form_id 
                            join public.user_registration ur on ur.email=id.observer_email
                            group by id.road_code,id.district order by last_uploaded_date desc;";

            var output = await sqlDataAccess.LoadData<Road, dynamic>(query, new { }, "Csmdb");

            return output;
        }

        public async Task<IEnumerable<Road>> GetRoads(string district)
        {
            string query = @"select id.road_code, id.district, max(id.uploaded_date) as uploaded_date from monitoring.initial_details id
                            join (select uuid from monitoring.construction_observation_detail group by uuid) c on c.uuid = id.form_id 
                            join public.user_registration ur on ur.email=id.observer_email
                            where id.district = @District
                            group by id.road_code,id.district order by uploaded_date desc;";

            var output = await sqlDataAccess.LoadData<Road, dynamic>(query, new { District = district }, "Csmdb");

            return output;
        }

        public async Task<IEnumerable<RoadDetails>> GetRoadDetails(string roadCode)
        {
            string query = @"select distinct id.form_id, id.ini_id,id.road_code,id.road_name,id.district,id.date,id.uploaded_date,ur.observer_name,id.observer_email,
                            ur.designation,id.report_status from monitoring.initial_details id
                            join monitoring.construction_observation_detail cod on cod.uuid=id.form_id             
                            left join monitoring.file f on f.form_id=cod.form_id
                            join public.user_registration ur on ur.email=id.observer_email
                            where id.road_code = @RoadCode order by id.ini_id DESC;";

            var output = await sqlDataAccess.LoadData<RoadDetails, dynamic>(query, new { RoadCode = roadCode }, "Csmdb");
            return output;

        }

        public async Task<IEnumerable<RoadDetails>> GetRoadDetails(string roadCode, string district)
        {
            string query = @"select distinct id.form_id, id.ini_id,id.road_code,id.road_name,id.district,id.date,id.uploaded_date,ur.observer_name,id.observer_email,
                            ur.designation,id.report_status from monitoring.initial_details id
                            join monitoring.construction_observation_detail cod on cod.uuid=id.form_id             
                            left join monitoring.file f on f.form_id=cod.form_id
                            join public.user_registration ur on ur.email=id.observer_email
                            where id.road_code = @RoadCode and id.district = @District order by id.ini_id DESC;";

            var parameters = new
            {
                RoadCode = roadCode,
                District = district
            };

            var output = await sqlDataAccess.LoadData<RoadDetails, dynamic>(query, parameters, "Csmdb");
            return output;

        }

        public async Task<IEnumerable<ReportDataModel>> GetReportDataList(string form_id, string roadeCode, string observerEmail)
        {
            string query = @"select id.ini_id,id.road_code,id.road_name,id.district,id.date,ur.observer_name,id.observer_email,ur.designation,cod.cons_id,
                            cod.form_id,cod.construction_type,cod.location_type,cod.location,cod.observation_notes,cod.quality_rating,cod.latitude,
                            cod.longitude,cod.line_latitude_from,cod.line_longitude_from,cod.line_latitude_to,cod.line_longitude_to,cod.altitude,f.file_id,
                            f.file_name,f.file_note,f.unique_file,f.file_type from monitoring.initial_details id
                            join monitoring.construction_observation_detail cod on cod.uuid=id.form_id            
                            left join monitoring.file f on f.form_id=cod.form_id
                            join public.user_registration ur on ur.email=id.observer_email
                            where id.form_id = @FormId and id.road_code = @RoadCode and  id.observer_email like @Email order by cod.cons_id";

            var parameters = new
            {
                FormId = form_id,
                RoadCode = roadeCode,
                Email = observerEmail
            };

            var output = await sqlDataAccess.LoadData<ReportDataModel, dynamic>(query, parameters, "Csmdb");
            return output;
        }

        public async Task<IEnumerable<Inital>> CheckReportEmail(string form_id, string roadeCode, string observerEmail)
        {
            string query = "select * from monitoring.initial_details where form_id=@FormId and road_code=@RoadCode and observer_email=@Email";

            var parameters = new
            {
                FormId = form_id,
                RoadCode = roadeCode,
                Email = observerEmail
            };

            var output = await sqlDataAccess.LoadData<Inital, dynamic>(query, parameters, "Csmdb");
            return output;
        }

        public async Task<int> UpdateReportStatus(string form_id, string roadCode, string observerEmail)
        {
            string query = "update monitoring.initial_details set report_status='1' where form_id = @FormId and road_code=@RoadCode and observer_email=@Email";

            var parameters = new
            {
                FormId = form_id,
                RoadCode = roadCode,
                Email = observerEmail
            };

            try
            {
                return await sqlDataAccess.ExecuteRow<dynamic>(query, parameters, "Csmdb");
            }
            catch (Exception)
            {
                //TODO : Log this issue.
                return 0;
            }
        }

        public async Task<GenericReport<T, U, R>> GetWholeReport<T, U, R>(string form_id, string road_code)
        {
            string queryInitial = "select * from monitoring.initial_details where form_id = @FormId and road_code = @RoadCode order by ini_id asc";
            string queryConstrunction = "select * from monitoring.construction_observation_detail where uuid = @FormId order by cons_id asc";
            string queryFiles = "select * from monitoring.file where uuid = @FormId order by file_type asc";

            var parameters = new
            {
                FormId = form_id,
                RoadCode = road_code
            };
            GenericReport<T, U, R> report = new GenericReport<T, U, R>
            {
                GetInitial = await sqlDataAccess.LoadData<T, dynamic>(queryInitial, parameters, "Csmdb"),
                GetConstruction = await sqlDataAccess.LoadData<U, dynamic>(queryConstrunction, parameters, "Csmdb"),
                GetFiles = await sqlDataAccess.LoadData<R, dynamic>(queryFiles, parameters, "Csmdb")
            };

            return report;
        }

        public async Task<int> UpdateConstructionObservation(ConstructionObservation constructionObservation)
        {
            string query = @"update monitoring.construction_observation_detail set construction_type = @ConsType,
                                location=@LocAtion, observation_notes = @ObservNotes, quality_rating = @QualityRating where form_id = @FormId";
            var parameters = new
            {
                ConsType = constructionObservation.construction_type,
                LocAtion = constructionObservation.location,
                ObservNotes = constructionObservation.observation_notes,
                QualityRating = constructionObservation.quality_rating,
                FormId = constructionObservation.form_id
            };

            try
            {
                return await sqlDataAccess.ExecuteRow<dynamic>(query, parameters, "Csmdb");
            }
            catch (Exception)
            {
                //TODO : Log this issue
                return 0;
            }
        }

        public async Task<bool> DeleteReportObservation(string form_id, string road_code)
        {
            string queryInitial = @"delete from monitoring.initial_details where form_id = @FormId and road_code = @RoadCode";
            string queryConstruction = @"delete from monitoring.construction_observation_detail where uuid = @FormId and road_code = @RoadCode";
            string queryFile = @"delete from monitoring.file where uuid = @FormId";

            var parameters = new
            {
                FormId = form_id,
                RoadCode = road_code
            };

            try
            {
                await sqlDataAccess.ExecuteRow<dynamic>(queryInitial, parameters, "Csmdb");
                await sqlDataAccess.ExecuteRow<dynamic>(queryConstruction, parameters, "Csmdb");
                await sqlDataAccess.ExecuteRow<dynamic>(queryFile, parameters, "Csmdb");
                return true;
            }
            catch (Exception)
            {
                // TODO: Log this issue;
                return false;
            }
            
        }

    }
}
