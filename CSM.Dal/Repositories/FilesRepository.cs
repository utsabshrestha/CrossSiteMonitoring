using CSM.Dal.Entities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace CSM.Dal.Repositories
{
    internal class FilesRepository : RepositoryBase, IFilesRepository
    {
        public FilesRepository(IDbTransaction transaction) : base(transaction)
        {

        }

        public async Task Add(IEnumerable<Files> files)
        {
            string sql = @"insert into monitoring.file 
                        (uuid,form_id,file_name,file_note,unique_file,file_type) 
                        values( @uuid, @form_id, @file_name, @file_note, @unique_file, @file_type)";

            await Connection.ExecuteAsync(sql, files, transaction: Transaction);
        }


        public async Task Delete(string form_id)
        {
            string sql = @"delete from monitoring.file where uuid = @FormId";
            await Connection.ExecuteAsync(sql, new { FormId = form_id }, transaction: Transaction);
        }

    }
}
