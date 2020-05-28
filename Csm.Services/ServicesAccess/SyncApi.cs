using Csm.Services.ServiceInterface;
using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using DataAccessLibrary.DataAccessLayer.Interfaces;
using System.Linq;
using System.Net;
using System.Data;

namespace Csm.Services.ServicesAccess
{
    public class SyncApi : ISyncApi
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ISqlDataAccess sqlDataAccess;
        private readonly ISqlLiteDataAccess sqlLiteDataAccess;
        private SyncStatus status;
        public string SqlLitePath { get; set; }

        public SyncApi(IWebHostEnvironment webHostEnvironment, ISqlDataAccess sqlDataAccess, ISqlLiteDataAccess sqlLiteDataAccess)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.sqlDataAccess = sqlDataAccess;
            this.sqlLiteDataAccess = sqlLiteDataAccess;
            status = new SyncStatus();
        }

        public async Task<SyncStatus> SyncData(SyncApiCred apiCred)
        {
            bool fileStatus = await sqlitedbWrite(apiCred.dbfile);

            if (fileStatus == false)
            {
                status.setStatus(SyncStatus.stat.Failed);
                return status;
            }

            bool dataSync = await SaveData();
            if (dataSync == false)
            {
                status.setStatus(SyncStatus.stat.Failed);
                return status; 
            }

            bool imageExtract = await ImageExtracter();
            if (imageExtract == true)
            {
                status.Message = "Uploaded";
            }

            status.setStatus(SyncStatus.stat.Success);
            return status;
        }


        public void Inst()
        {
            List<dynamic> id = new List<dynamic> {
            new {
                date =System.DateTime.Now,
                observer_name = "utsab",
                designation = "designer",
                form_id = "123321",
                observer_emai = "abc#top.com",
                road_code = "1234234",
                road_name = "godha road",
                district = "Ramechaap",
                report_status = 4,
                uploaded_date = System.DateTime.Now,
                is_test = true
            },
            new {
                date =System.DateTime.Now,
                observer_name = "utsab",
                designation = "designer",
                form_id = "123321",
                observer_emai = "abc#top.com",
                road_code = "1234234",
                road_name = "godha road",
                district = "Ramechaap",
                report_status = 4,
                uploaded_date = System.DateTime.Now,
                is_test = true
            },
            new {
                date =System.DateTime.Now,
                observer_name = "utsab",
                designation = "designer",
                form_id = "123321",
                observer_emai = "abc#top.com",
                road_code = "1234234",
                road_name = "godha road",
                district = "Ramechaap",
                report_status = 4,
                uploaded_date = System.DateTime.Now,
                is_test = true
            }
            };


            string queyy = @"select monitoring.sp_initial_details(@date, @observer_name,@designation,@form_id,@observer_emai,@road_code,
                            @road_name,@district,@report_status,@uploaded_date,@is_test)";
            sqlDataAccess.InsertSp<dynamic>(queyy, id, "Csmdb");
        }
        private async Task<bool> SaveData()
        {
            IEnumerable<Inital> initals = await LoadinitalDetails();
            IEnumerable<ConstructionObservation> constructionObservations = await LoadConstructionObservationsDetails();
            IEnumerable<Files> files = await LoadFiles();
            IEnumerable<EventRecording> eventRecordings = await LoadEvents();
            try
            {
                sqlDataAccess.StartTransaction("Csmdb");

                foreach (var inital in initals)
                {
                    if ( !(await InitalExistence(inital.form_id)))
                    {
                        await InsertInitial(inital);
                        await InsertConstructionObservationD(constructionObservations, inital.form_id);
                        await InsertFile(files, inital.form_id);
                        await InsertEventsRecording(eventRecordings, inital.form_id);
                    }
                    else
                    {
                        await UpdateInitial(inital);
                        await UpdateConstructionObservationDandFiles(constructionObservations, inital.form_id, files);
                        await UpdateEventsRecording(eventRecordings, inital.form_id);
                    }
                }

                sqlDataAccess.CommintTransaction();
                return true;
            }
            catch (Exception e)
            {
                sqlDataAccess.RollbackTransaction();
                status.Message = e.Message.ToString();
                return false;
            }
        }

        private async Task<bool> InitalExistence(string formId)
        {
            string query = @"select * from monitoring.initial_details where form_id = @FomrId";

            var parameter = new
            {
                FomrId = formId
            };

            IEnumerable<Inital> initals = await sqlDataAccess.LoadData<Inital, dynamic>(query, parameter, "Csmdb");

            if ((initals != null) && (!initals.Any()))
            {
                return true;
            }

            return false;
        }

        private async Task InsertInitial(Inital inital)
        {
            try
            {
               await sqlDataAccess.SaveDataInTransaction<Inital>(InitialQuery, inital);
            }
            catch
            {
                throw;
            }
        }

        private async Task UpdateInitial(Inital inital)
        {
            try
            {
                await sqlDataAccess.SaveDataInTransaction<Inital>(InitialQueryUpdate, inital);
            }
            catch
            {
                throw;
            }
        }

        private async Task InsertConstructionObservationD(IEnumerable<ConstructionObservation> constructionObservations , string uuid)
        {
            foreach(ConstructionObservation observation in constructionObservations.Where(e => e.uuid == uuid))
            {
                observation.construction_type.Replace('\'',' ');
                observation.observation_notes.Replace('\'',' ');
                observation.location.Replace('\'',' ');

                if(observation.location_type == "Point Location")
                {
                    LocationImage(observation.form_id, observation.latitude, observation.longitude);
                    try
                    {
                       await sqlDataAccess.SaveDataInTransaction(PointQuery, observation);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                else if(observation.location_type == "Line Location")
                {
                    if (observation.line_latitude_to < 26 || observation.line_latitude_to > 31)
                        observation.line_latitude_to = observation.line_latitude_from;

                    if (observation.line_longitude_to < 81 || observation.line_longitude_to > 89)
                        observation.line_longitude_to = observation.line_longitude_from;

                    LocationImage(observation.form_id, observation.line_latitude_from, observation.line_latitude_to, observation.line_longitude_from, observation.line_longitude_to);
                    try
                    {
                      await  sqlDataAccess.SaveDataInTransaction(LineQuery, observation);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }

        private async Task UpdateConstructionObservationDandFiles(IEnumerable<ConstructionObservation> constructionObservations, string uuid, IEnumerable<Files> files)
        {
            List<Files> Newfiles = new List<Files>();
            foreach (ConstructionObservation observation in constructionObservations.Where(e => e.uuid == uuid))
            {
                observation.construction_type.Replace('\'', ' ');
                observation.observation_notes.Replace('\'', ' ');
                observation.location.Replace('\'', ' ');

                if ((await ConstructionObservationExistence(observation.form_id)))
                {
                    try
                    {
                        await sqlDataAccess.SaveDataInTransaction(ConstructionUpdate, observation);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                else
                {
                    if (observation.location_type == "Point Location")
                    {
                        LocationImage(observation.form_id, observation.latitude, observation.longitude);
                        try
                        {
                            await sqlDataAccess.SaveDataInTransaction(PointQuery, observation);
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                    else if (observation.location_type == "Line Location")
                    {
                        if (observation.line_latitude_to < 26 || observation.line_latitude_to > 31)
                            observation.line_latitude_to = observation.line_latitude_from;

                        if (observation.line_longitude_to < 81 || observation.line_longitude_to > 89)
                            observation.line_longitude_to = observation.line_longitude_from;

                        LocationImage(observation.form_id, observation.line_latitude_from, observation.line_latitude_to, observation.line_longitude_from, observation.line_longitude_to);
                        try
                        {
                            await sqlDataAccess.SaveDataInTransaction(LineQuery, observation);
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                    Newfiles.Add(files.FirstOrDefault(x => x.form_id == observation.form_id));
                }
            }
            await InsertFile(Newfiles, uuid);
        }

        private async Task<bool> ConstructionObservationExistence(string formId)
        {
            string query = @"select * from monitoring.construction_observation_detail where form_id = @FomrId";

            var parameter = new
            {
                FomrId = formId
            };

            IEnumerable<ConstructionObservation> initals = await sqlDataAccess.LoadData<ConstructionObservation, dynamic>(query, parameter, "Csmdb");

            if ((initals != null) && (!initals.Any()))
            {
                return true;
            }

            return false;
        }

        private async Task InsertFile(IEnumerable<Files> files, string uuid)
        {
            foreach(Files file in files.Where(x => x.uuid == uuid))
            {
                try
                {
                   await sqlDataAccess.SaveDataInTransaction(FileQuery, file);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private async Task<bool> ImageExtracter()
        {
            string filename;
            try
            {
                string query = "select file_name,blob_file from file";
                DataTable imageTable = sqlLiteDataAccess.LoadSqLiteBlob(query, SqlLitePath);
                List<Task> ImageExtract = new List<Task>();
                foreach(DataRow row in imageTable.Rows)
                {
                    filename = Path.Combine(webHostEnvironment.WebRootPath, "AppImages", row["file_name"].ToString());
                    if (row["blob_file"] != DBNull.Value)
                    {
                        ImageExtract.Add(System.IO.File.WriteAllBytesAsync(filename, (byte[])row["blob_file"]));
                    }
                }
                await Task.WhenAll(ImageExtract);
                return true;
            }
            catch (Exception e)
            {
                status.Message = e.Message.ToString();
                return false;
            }
        }

        private async Task InsertEventsRecording(IEnumerable<EventRecording> eventRecordings, string uuid)
        {
            foreach(EventRecording eventRecording in eventRecordings.Where(x => x.uuid == uuid))
            {
                try
                {
                    LocationImage(eventRecording.form_id, eventRecording.latitude, eventRecording.longitude);
                    await sqlDataAccess.SaveDataInTransaction(EventsQuery, eventRecording);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }       
        
        private async Task UpdateEventsRecording(IEnumerable<EventRecording> eventRecordings, string uuid)
        {
            foreach(EventRecording eventRecording in eventRecordings.Where(x => x.uuid == uuid))
            {
                if(await EventsExistence(eventRecording.form_id))
                {
                    try
                    {
                        await sqlDataAccess.SaveDataInTransaction(EventsQueryUpdate, eventRecording);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                else
                {
                    try
                    {
                        LocationImage(eventRecording.form_id, eventRecording.latitude, eventRecording.longitude);
                        await sqlDataAccess.SaveDataInTransaction(EventsQuery, eventRecording);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                
            }
        }

        private async Task<bool> EventsExistence(string formId)
        {
            string query = @"select * from monitoring.event_recording where form_id = @FomrId";

            var parameter = new
            {
                FomrId = formId
            };

            IEnumerable<EventRecording> initals = await sqlDataAccess.LoadData<EventRecording, dynamic>(query, parameter, "Csmdb");

            if ((initals != null) && (!initals.Any()))
            {
                return true;
            }

            return false;
        }

        private string InitialQuery = @"INSERT INTO monitoring.initial_details (date, observer_name, designation, form_id, observer_email, road_code, 
            road_name, district, report_status, uploaded_date, is_test) VALUES (@date, @observer_name ,@designation ,@form_id ,@observer_email 
            ,@road_code ,@road_name ,@district ,@report_status ,@uploaded_date ,@is_test)";

        private string InitialQueryUpdate = @"UPDATE monitoring.initial_details SET date = @date, observer_name = @observer_name, designation =@designation 
            , form_id = @form_id, observer_email = @observer_email , road_code = @road_code , road_name =@road_name , district = @district, 
            report_status =@report_status, uploaded_date = @uploaded_date, is_test =@is_test WHERE form_id=@form_id";

        private string ConstructionUpdate = @"UPDATE monitoring.construction_observation_detail SET road_code=@road_code, construction_type=@construction_type,
        location=@location, observation_notes=@observation_notes, quality_rating=@quality_rating, latitude=@latitude, longitude=@longitude, 
        line_latitude_from= @line_latitude_from, line_longitude_from=@line_longitude_from, line_latitude_to=@line_latitude_to, line_longitude_to=@line_longitude_to, 
        altitude=@altitude, date=@date where form_id= @form_id";

        private readonly string PointQuery = @"INSERT INTO monitoring.construction_observation_detail 
            (uuid, form_id, construction_type, location, observation_notes, quality_rating, latitude,longitude, altitude,date, road_code,location_type, 
            point_geom) VALUES 
            (@uuid, @form_id, @construction_type, @location, @observation_notes, @quality_rating, @latitude,@longitude,@altitude,@date,@road_code,@location_type,
            ST_SetSRID(ST_MakePoint(longitude,latitude),4326)";

        private readonly string LineQuery = @"INSERT INTO monitoring.construction_observation_detail 
            (uuid, form_id, construction_type, location, observation_notes, quality_rating, altitude,date, road_code,location_type, 
            the_geom,line_latitude_from,line_longitude_from,line_latitude_to,line_longitude_to) VALUES 
            (@uuid, @form_id, @construction_type, @location, @observation_notes, @quality_rating, @altitude,@date,@road_code,@location_type,
            ST_SetSRID(ST_MakeLine(ST_MakePoint(line_longitude_from,line_latitude_from), ST_MakePoint(line_longitude_to,line_latitude_to)),4326),
            @line_latitude_from,@line_longitude_from,@line_latitude_to,@line_longitude_to)";

        private readonly string FileQuery = @"insert into monitoring.file 
            (uuid,form_id,file_name,file_note,unique_file,file_type) 
            values( @uuid, @form_id, @file_name, @file_note, @unique_file, @file_type)";

        private readonly string EventsQuery = @"insert into monitoring.event_recording
            (uuid,district,road_link ,river_name,latitude,longitude,division,events,remarks,observations, msg_priority,form_id, date, road_code) 
            values( @uuid, @district, @road_link, @river_name, @latitude, @longitude, @division, @events, @remarks,
            @observations, @msg_priority, @form_id, @date, @road_code)";
        
        private readonly string EventsQueryUpdate = @"UPDATE monitoring.event_recording SET
            district = @district,road_link = @road_link,river_name = @river_name,latitude = @latitude,longitude = @longitude,division = @division,events = @events,
            remarks = @remarks,observations = @observations, msg_priority = @msg_priority,form_id =  @form_id, date = @date, road_code = @road_code
            WHERE form_id=@form_id";


        private void LocationImage(string filename, double poinLat, double pointLon)
        {
            string filePath = Path.Combine(webHostEnvironment.WebRootPath, "Mapping", filename + ".png");
            string url = $"http://maps.googleapis.com/maps/api/staticmap?center={poinLat},{pointLon}&&zoom=16&size=900x400&maptype=hybrid&markers=color:blue%7Clabel:C%7C{poinLat},{pointLon}&sensor=false&key=AIzaSyCmYOwvp5ltKtrOb_puR2Wm-4uAqae9Wa8";
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadDataAsync(new Uri(url), filePath);
            }
        }

        private void LocationImage(string filename, double lineLatFrm, double lineLatTo, double lineLonFrm, double lineLonTo)
        {
            string filePath = Path.Combine(webHostEnvironment.WebRootPath, "Mapping", filename + ".png");
            string url = $"http://maps.googleapis.com/maps/api/staticmap?center={lineLatFrm},{lineLonFrm}&&zoom=16&size=900x400&maptype=hybrid&markers=color:green%7Clabel:S%7C{lineLatFrm},{lineLonFrm}&markers=color:red%7Clabel:E%7C{lineLatTo},{lineLonTo}&sensor=false&key=AIzaSyCmYOwvp5ltKtrOb_puR2Wm-4uAqae9Wa8";
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile(new Uri(url), filePath);
            }
        }

        private async Task<bool> sqlitedbWrite(IFormFile file)
        {
            SqlLitePath = Path.Combine(webHostEnvironment.WebRootPath, "sqlFiles", file.FileName);

            try
            {
                if (System.IO.File.Exists(SqlLitePath))
                {
                    System.IO.File.Delete(SqlLitePath);
                }

                using (FileStream stream = new FileStream(SqlLitePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                if (System.IO.File.Exists(SqlLitePath))
                {
                    return true;
                }
                else
                {
                    status.Message = "File unable to create";
                    return false;
                }
            }
            catch (UnauthorizedAccessException e)
            {
                status.Message = e.Message.ToString();
                return false;
            }
            catch (FileNotFoundException e)
            {
                status.Message = e.Message.ToString();
                return false;
            }
            catch (IOException e)
            {
                status.Message = e.Message.ToString();
                return false;
            }
            catch (Exception e)
            {
                status.Message = e.Message.ToString();
                return false;
            }
        }



        private async Task<IEnumerable<Inital>> LoadinitalDetails()
        {
            string query = @"select * from initial_details";

            return await sqlLiteDataAccess.LoadSqLiteData<Inital, dynamic>(query, new { }, SqlLitePath);
        }

        private async Task<IEnumerable<ConstructionObservation>> LoadConstructionObservationsDetails()
        {
            string query = @"select * from construction_observation_detail";

            return await sqlLiteDataAccess.LoadSqLiteData<ConstructionObservation, dynamic>(query, new { }, SqlLitePath);
        }

        private async Task<IEnumerable<Files>> LoadFiles()
        {
            string query = @"select file_id, form_id, file_name, file_note, unique_file, file_type from file";

            return await sqlLiteDataAccess.LoadSqLiteData<Files, dynamic>(query, new { }, SqlLitePath);
        }

        private async Task<IEnumerable<EventRecording>> LoadEvents()
        {
            string query = @"SELECT * FROM event_recording";

            return await sqlLiteDataAccess.LoadSqLiteData<EventRecording, dynamic>(query, new { }, SqlLitePath);
        }
    }
}
