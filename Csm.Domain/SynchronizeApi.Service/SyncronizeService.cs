using Csm.Dto.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csm.Domain.SynchronizeApi.Service
{
    public class SyncronizeService : ISyncronizeService
    {
        private readonly ICreateFile createFile;
        private readonly ISynchronizer synchronizer;
        private readonly IImageExtractor imageExtractor;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ISqlitePath sqlitePath;

        public SyncronizeService
            (
                ICreateFile createFile,
                ISynchronizer synchronizer,
                IImageExtractor imageExtractor,
                IWebHostEnvironment webHostEnvironment,
                ISqlitePath sqlitePath
            )
        {
            this.createFile = createFile;
            this.synchronizer = synchronizer;
            this.imageExtractor = imageExtractor;
            this.webHostEnvironment = webHostEnvironment;
            this.sqlitePath = sqlitePath;
        }

        //TODO: 
        // 1) write the IForm file and then.
        // 2) read the sqlite file. & write the sqlite table to db.
        // 3) Extract the media files.
        public async Task<bool> Syncronize(SyncApiCred syncApiCred)
        {
            setPath(syncApiCred.formFile.FileName , syncApiCred.username);
            var writefile = await createFile.WriteFile(syncApiCred.formFile);
            var syncfile = false;
            if (writefile)
            {
                syncfile = await synchronizer.SynchronizeData();
                await imageExtractor.BlobImageWriter();
                await imageExtractor.GoogleLocationImageWriter();
            }
            return syncfile;
        }

        private void setPath(string FileName, string username)
        {
            sqlitePath.setPath = Path.Combine(webHostEnvironment.WebRootPath, "CsmSqliteFiles", String.Concat(username, '_', FileName));
        }

    }
}