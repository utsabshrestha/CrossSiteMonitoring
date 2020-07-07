using CSM.Dal.Entities;
using CSM.Dal.Internal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSM.Dal.Repositories
{
    public class MonitoringRepository : IMonitoringRepository
    {
        private readonly IDataAccess dataAccess;

        public MonitoringRepository(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public async Task<IEnumerable<District>> GetDistricts()
        {
            string sql = @"select id, district_name, dcode from monitoring.tbl_district order by district_name asc;";

            var output = await dataAccess.LoadData<District, dynamic>(sql, new { });
            return output;
        }

        public async Task<IEnumerable<District>> GetUserSpecificDistircts(string observer_email)
        {
            string query = @"select distinct district_name,id,dcode from monitoring.tbl_district d
                            join monitoring.initial_details id on id.district = d.district_name
                            where id.observer_email = @ObserverEmail order by district_name asc";
            var parameters = new
            {
                ObserverEmail = observer_email
            };
            var output = await dataAccess.LoadData<District, dynamic>(query, parameters);
            return output;
        }

        public async Task<IEnumerable<Road>> GetRoads()
        {
            string query = @"select id.road_code, max(id.district) as district, max(id.uploaded_date) as last_uploaded_date 
                            from monitoring.initial_details id
                            join (select uuid from monitoring.construction_observation_detail group by uuid) c on c.uuid = id.form_id 
                            join public.user_registration ur on ur.email=id.observer_email
                            group by id.road_code order by last_uploaded_date desc;";

            var output = await dataAccess.LoadData<Road, dynamic>(query, new { });
            return output;
        }

        public async Task<IEnumerable<Road>> GetRoads(string district)
        {
            string query = @"select id.road_code, id.district, max(id.uploaded_date) as uploaded_date from monitoring.initial_details id
                            join (select uuid from monitoring.construction_observation_detail group by uuid) c on c.uuid = id.form_id 
                            join public.user_registration ur on ur.email=id.observer_email
                            where id.district = @District
                            group by id.road_code,id.district order by uploaded_date desc;";

            var output = await dataAccess.LoadData<Road, dynamic>(query, new { District = district });
            return output;
        }

        public async Task<IEnumerable<Road>> GetUserSpecificRoads(string observer_email)
        {
            string query = @"select id.road_code, id.district, max(id.uploaded_date) as last_uploaded_date 
                            from monitoring.initial_details id
                            join (select uuid from monitoring.construction_observation_detail group by uuid) c on c.uuid = id.form_id 
                            join public.user_registration ur on ur.email=id.observer_email
                            where id.observer_email = @ObserverEmail
                            group by id.road_code,id.district order by last_uploaded_date desc;";
            var parameters = new
            {
                ObserverEmail = observer_email
            };
            var output = await dataAccess.LoadData<Road, dynamic>(query, parameters);
            return output;
        }

        public async Task<IEnumerable<Road>> GetUserSpecificRoads(string observer_email, string district)
        {
            string query = @"select id.road_code, id.district, max(id.uploaded_date) as last_uploaded_date 
                            from monitoring.initial_details id
                            join (select uuid from monitoring.construction_observation_detail group by uuid) c on c.uuid = id.form_id 
                            join public.user_registration ur on ur.email=id.observer_email
                            where id.observer_email = @ObserverEmail and id.district = @District
                            group by id.road_code,id.district order by last_uploaded_date desc;";
            var parameters = new
            {
                ObserverEmail = observer_email,
                District = district
            };
            var output = await dataAccess.LoadData<Road, dynamic>(query, parameters);
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

            var output = await dataAccess.LoadData<RoadDetails, dynamic>(query, new { RoadCode = roadCode });
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
            var output = await dataAccess.LoadData<RoadDetails, dynamic>(query, parameters);
            return output;
        }

        public async Task<IEnumerable<Initial>> CheckReport(string form_id, string roadeCode, string observerEmail)
        {
            string query = "select * from monitoring.initial_details where form_id=@FormId and road_code=@RoadCode and observer_email=@Email";
            var parameters = new
            {
                FormId = form_id,
                RoadCode = roadeCode,
                Email = observerEmail
            };
            var output = await dataAccess.LoadData<Initial, dynamic>(query, parameters);
            return output;
        }

        public async Task<GenericReport<T, U, R>> GetWholeReport<T,U,R>(string form_id, string road_code)
            where T : class where U : class where R : class
        {
            string sql = @"select * from monitoring.initial_details where form_id = @FormId and road_code = @RoadCode order by ini_id asc; 
                        select * from monitoring.construction_observation_detail where uuid = @FormId order by cons_id asc; 
                        select * from monitoring.file where uuid = @FormId order by file_type asc;";
            var parameters = new
            {
                FormId = form_id,
                RoadCode = road_code
            };

            var report = await dataAccess.getReport<T, U, R, dynamic>(sql, parameters);
            return report;
            // TODO: PUT THIS IN DATAACCESS IF POSSIBLE - Completed!
        }

    }
}