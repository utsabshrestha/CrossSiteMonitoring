using Csm.Dto.Entities;
using CSM.Dal.Entities;
using CSM.Dal.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Csm.Domain.SynchronizeApi.Service
{
    public class ImageExtractor : IImageExtractor
    {
        private readonly IReadSqlite readSqlite;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ILogger<ImageExtractor> logger;
        private readonly ISqlitePath sqlitePath;

        public ImageExtractor(
            IReadSqlite readSqlite, 
            IWebHostEnvironment webHostEnvironment,
            ILogger<ImageExtractor> logger,
            ISqlitePath sqlitePath
            )
        {
            this.readSqlite = readSqlite;
            this.webHostEnvironment = webHostEnvironment;
            this.logger = logger;
            this.sqlitePath = sqlitePath;
        }

        public async Task BlobImageWriter()
        {
            DataTable dataTable = readSqlite.getBlobImage(sqlitePath.path);
            List<Task> tasks = new List<Task>();
            try
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    //TODO : set IMAGE LOCATION - Completed!.
                    string fileName = Path.Combine(webHostEnvironment.WebRootPath, "CsmImages", dataRow["file_name"].ToString());
                    if (dataRow["blob_file"] != DBNull.Value)
                    {
                        tasks.Add(System.IO.File.WriteAllBytesAsync(fileName, (byte[])dataRow["blob_file"]));
                    }
                }
                await Task.WhenAll(tasks);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error while extracting blob images : {msg}", e.Message);
            }
        }

        public async Task GoogleLocationImageWriter()
        {
            IEnumerable<ConstructionObservation> constructions = await readSqlite.getConstructionObservations(sqlitePath.path);
            try
            {
                foreach (var observation in constructions)
                {
                    if (observation.location_type == "Point Location")
                        WriteLocationImage(observation.form_id, observation.latitude, observation.longitude);
                    else
                        WriteLocationImage(observation.form_id, observation.line_latitude_from, observation.line_latitude_to, observation.line_longitude_from, observation.line_longitude_to);
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Erorr during writeing google location img : {msg}", e.Message);
            }

        }

        //TODO :  set IMAGE GoogleLocationImage - Completed!.
        private void WriteLocationImage(string filename, double poinLat, double pointLon)
        {
            string filePath = Path.Combine(webHostEnvironment.WebRootPath, "GoogleLocationImage", filename + ".png");
            string url = $"http://maps.googleapis.com/maps/api/staticmap?center={poinLat},{pointLon}&&zoom=16&size=900x400&maptype=hybrid&markers=color:blue%7Clabel:C%7C{poinLat},{pointLon}&sensor=false&key=AIzaSyCmYOwvp5ltKtrOb_puR2Wm-4uAqae9Wa8";
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile(new Uri(url), filePath);
            }
        }

        private void WriteLocationImage(string filename, double lineLatFrm, double lineLatTo, double lineLonFrm, double lineLonTo)
        {
            string filePath = Path.Combine(webHostEnvironment.WebRootPath, "GoogleLocationImage", filename + ".png");
            string url = $"http://maps.googleapis.com/maps/api/staticmap?center={lineLatFrm},{lineLonFrm}&&zoom=16&size=900x400&maptype=hybrid&markers=color:green%7Clabel:S%7C{lineLatFrm},{lineLonFrm}&markers=color:red%7Clabel:E%7C{lineLatTo},{lineLonTo}&sensor=false&key=AIzaSyCmYOwvp5ltKtrOb_puR2Wm-4uAqae9Wa8";
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile(new Uri(url), filePath);
            }
        }
    }
}