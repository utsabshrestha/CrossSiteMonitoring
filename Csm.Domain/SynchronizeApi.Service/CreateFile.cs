using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Csm.Dto.Entities;

namespace Csm.Domain.SynchronizeApi.Service
{
    public class CreateFile : ICreateFile
    {
        //IWebHostEnvironment is added by including framework referecne Microsoft.AspNetCore.App in project file.
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ILogger<CreateFile> logger;
        private readonly ISqlitePath sqlitePath;

        public CreateFile(
            IWebHostEnvironment webHostEnvironment,
            ILogger<CreateFile> logger,
            ISqlitePath sqlitePath
            )
        {
            this.webHostEnvironment = webHostEnvironment;
            this.logger = logger;
            this.sqlitePath = sqlitePath;
        }

        public async Task<bool> WriteFile(IFormFile formFile)
        {
            string fileStorePath = sqlitePath.path;
            try
            {
                ifFileExistDeleteIt(fileStorePath);
                if (formFile.Length > 0)
                {
                    using (var stram = System.IO.File.Create(fileStorePath))
                    {
                        await formFile.CopyToAsync(stram);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error occured while writing file: {Message}", e.Message);
                return false;
            }
        }

        private void ifFileExistDeleteIt(string path)
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }
    }
}
