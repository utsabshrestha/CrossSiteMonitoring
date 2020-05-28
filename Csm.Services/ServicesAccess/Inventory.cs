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
            string query = @"select max(id.ini_id)as id,id.road_code,max(id.road_name) as road_name,max(id.district) as district,max(id.date) as date,
                            max(id.uploaded_date) as uploaded_date from monitoring.initial_details id
                            join(select initials_id from monitoring.construction_observation_detail group by initials_id)cod on cod.initials_id=id.ini_id
                            group by id.road_code order by uploaded_date DESC;";

            var output = await sqlDataAccess.LoadData<Road, dynamic>(query, new { }, "Csmdb");

            return output;
        }

        public async Task<IEnumerable<Road>> GetRoads(string district)
        {
            string query = @"select max(id.ini_id)as id,id.road_code,max(id.road_name) as road_name,max(id.district) as district,max(id.date) as date,
                            max(id.uploaded_date) as uploaded_date from monitoring.initial_details id
                            join(select initials_id from monitoring.construction_observation_detail group by initials_id)cod on cod.initials_id=id.ini_id
                            where id.district = @District
                            group by id.road_code order by uploaded_date DESC;";

            var output = await sqlDataAccess.LoadData<Road, dynamic>(query, new { District = district }, "Csmdb");

            return output;
        }

        public async Task<IEnumerable<RoadDetails>> GetRoadDetails(string roadCode)
        {
            string query = @"select id.ini_id,id.road_code,id.road_name,id.district,id.date,id.uploaded_date,ur.observer_name,id.observer_email,
                            ur.designation,id.report_status from monitoring.initial_details id
                            join(select * from monitoring.construction_observation_detail)cod on cod.initials_id=id.ini_id          
                            left join(select * from monitoring.file)f on f.form_id=cod.form_id
                            join(select * from public.user_registration)ur on ur.email=id.observer_email
                            where id.road_code = @RoadCode order by id.ini_id DESC;";

            var output = await sqlDataAccess.LoadData<RoadDetails, dynamic>(query, new { RoadCode = roadCode }, "Csmdb");
            return output;

        }

        public async Task<IEnumerable<RoadDetails>> GetRoadDetails(string roadCode, string district)
        {
            string query = @"select id.ini_id,id.road_code,id.road_name,id.district,id.date,id.uploaded_date,ur.observer_name,id.observer_email,
                            ur.designation,id.report_status from monitoring.initial_details id
                            join(select * from monitoring.construction_observation_detail)cod on cod.initials_id=id.ini_id          
                            left join(select * from monitoring.file)f on f.form_id=cod.form_id
                            join(select * from public.user_registration)ur on ur.email=id.observer_email
                            where id.road_code = @RoadCode and id.district = @District order by id.ini_id DESC;";

            var parameters = new
            {
                RoadCode = roadCode,
                District = district
            };

            var output = await sqlDataAccess.LoadData<RoadDetails, dynamic>(query, parameters, "Csmdb");
            return output;

        }

        public async Task<IEnumerable<ReportDataModel>> GetReportDataList(string roadeCode, DateTime date, string observerEmail)
        {
            string query = @"select id.ini_id,id.road_code,id.road_name,id.district,id.date,ur.observer_name,id.observer_email,ur.designation,cod.cons_id,
                            cod.form_id,cod.construction_type,cod.location_type,cod.location,cod.observation_notes,cod.quality_rating,cod.latitude,
                            cod.longitude,cod.line_latitude_from,cod.line_longitude_from,cod.line_latitude_to,cod.line_longitude_to,cod.altitude,f.file_id,
                            f.file_name,f.file_note,f.unique_file,f.file_type from monitoring.initial_details id
                            join(select * from monitoring.construction_observation_detail)cod on cod.initials_id=id.ini_id            
                            left join(select * from monitoring.file)f on f.form_id=cod.form_id
                            join(select * from monitoring.user_registration)ur on ur.email=id.observer_email
                            where id.road_code = @RoadCode and id.date = @Date and id.observer_email like @Email order by cod.cons_id";

            var parameters = new
            {
                RoadCode = roadeCode,
                Date = date,
                Email = observerEmail
            };

            var output = await sqlDataAccess.LoadData<ReportDataModel, dynamic>(query, parameters, "Csmdb");
            return output;
        }
    }
}
