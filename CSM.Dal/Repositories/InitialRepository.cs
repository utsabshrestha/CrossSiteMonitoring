using CSM.Dal.Entities;
using Dapper;
using System.Data;
using System.Threading.Tasks;

namespace CSM.Dal.Repositories
{
    internal class InitialRepository : RepositoryBase, IInitialRepository
    {
        public InitialRepository(IDbTransaction transaction) : base(transaction)
        {
        }

        public async Task Add(Initial initial)
        {
            string sql = @"INSERT INTO monitoring.initial_details (date, observer_name, designation, form_id, observer_email, road_code, 
                                    road_name, district, report_status, uploaded_date, is_test) VALUES (@date, @observer_name ,@designation 
                                    ,@form_id ,@observer_email ,@road_code ,@road_name ,@district ,@report_status ,@uploaded_date ,@is_test)";

            await Connection.ExecuteAsync(sql, initial, transaction: Transaction);
        }

        public async Task Update(Initial initial)
        {
            string sql = @"UPDATE monitoring.initial_details SET date = @date, observer_name = @observer_name, designation =@designation 
                        , form_id = @form_id, observer_email = @observer_email , road_code = @road_code , road_name =@road_name , district = @district, 
                        report_status =@report_status, is_test =@is_test WHERE form_id=@form_id";
            await Connection.ExecuteAsync(sql, initial, transaction: Transaction);
        }

        public async Task Delete(string form_id, string road_code)
        {
            string sql = @"delete from monitoring.initial_details where form_id = @FormId and road_code = @RoadCode";
            await Connection.ExecuteAsync(sql, new { FormId = form_id, RoadCode = road_code }, transaction: Transaction);
        }

        public async Task<int> UpdateInitialStatus(string form_id, string roadCode, string observerEmail)
        {
            string sql = @"update monitoring.initial_details set report_status='1' 
                        where form_id = @FormId and road_code=@RoadCode and observer_email=@Email";
            var parameters = new
            {
                FormId = form_id,
                RoadCode = roadCode,
                Email = observerEmail
            };
            return await Connection.ExecuteAsync(sql, parameters, transaction: Transaction);
        }
    }
}