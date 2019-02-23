// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MagicSync.cs" company="CRM-Sensor">
//   Copyright (c) OMK-Processing / CRM-Sensor 
//   Created on 07.12.2016 17:41 by Albert Idiyatullin
// </copyright>
// <summary>
//   Я кайфоломщик, мне лень даже написать документацию к этому классу.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Web.Configuration;
using Common.Logging;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using NHibernate.Proxy;
using NHibernate.Util;
using Surveys.Core;
using Surveys.Core.Configuration;
using Surveys.Core.Helpers;
using Surveys.Core.Model;
using Surveys.Core.Model.Queue;
using Surveys.Core.Repositories;
using Surveys.Data.Content;
using Surveys.Data.NHibernate;
using Surveys.Data.NHibernate.Repositories;

namespace Surveys.Service.Core.Sync.FromHost
{
    public class WebApiSyncService
    {
        public struct SYSTEMTIME
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds;
        }

        [DllImport("kernel32.dll")]
        public extern static bool SetSystemTime(ref SYSTEMTIME lpSystemTime);

        #region Private methods

        private static readonly ILog logger = LogManager.GetLogger<WebApiSyncService>();

        private HttpClient client;

        private readonly string syncServerAddress = LocalConfiguration.GetSetting("SyncServerHost");

        private readonly string syncServerUserName = LocalConfiguration.GetSetting("SyncUser");

        private readonly string syncServerPassword = LocalConfiguration.GetSetting("SyncPassword");

        #endregion

        #region C-tor

        public WebApiSyncService()
        {
        }

        #endregion

        #region Dependencies

        [Dependency]
        public IUserContext UserContext { get; set; }

        [Dependency]
        public EntityMagicSync EntityMagicSync { get; set; }

        [Dependency]
        public ContentFileRepository ContentFileRepository { get; set; }

        [Dependency]
        public LocalContentStorage ContentStorage { get; set; }

        [Dependency]
        public IStyleInfoRepository StyleInfoRepository { get; set; }

        [Dependency]
        public IUnitRepository UnitRepository { get; set; }

        [Dependency]
        public IDeviceRepository DeviceRepository { get; set; }

        [Dependency]
        public IQueueServiceRepository QueueServiceRepository { get; set; }

        [Dependency]
        public IGroupRepository GroupRepository { get; set; }

        [Dependency]
        public IScreenplayPartRepository ScreenplayPartRepository { get; set; }

        #endregion

        #region Public methods

        public void SyncDataFromHost()
        {
            logger.Info("Sync data from host started");

            SyncCompany();
            SyncUnits();
            SyncRoles();
            SyncUsers();
            SyncEmployees();
            SyncStyles();
            SyncSurveys();
            SyncScreenplays();
            SyncQueueServiceGroups();
            SyncQueueServices();
            SyncQueueServiceRules();
            SyncGroups();
            SyncDevices();
            SyncReportGroups();
            SyncReports();
            SyncUnitsWithQueueServiceBindings();

            var unitCode = syncServerUserName.Replace("a", String.Empty);
            var unit = UnitRepository.Query().FirstOrDefault(x => x.Code == unitCode);

            if (unit != null)
            {
                SyncDateTime(unit.Id);
            }

            logger.Info("Sync data from host complete");
        }

        #endregion

        #region Help methods

        private void AddAuthHeaders(HttpRequestMessage request)
        {
            request.Headers.Add("X-Auth-User", syncServerUserName);
            request.Headers.Add("X-Auth-Password", syncServerPassword);
        }

        private void SyncEntity<TEntity>(string entityUrl, bool traceContent = true, bool traceJson = false) where TEntity : class, IEntity
        {
            ExecuteRequest(entityUrl, contentStr => DeserializeEntity<TEntity>(contentStr, null, traceJson), traceContent);
        }

        private void SyncEntity<TEntity>(string entityUrl, Action<TEntity> postSyncAction, bool traceContent = true, bool traceJson = false) where TEntity : class, IEntity
        {
            ExecuteRequest(entityUrl, contentStr => DeserializeEntity<TEntity>(contentStr, postSyncAction, traceJson), traceContent);
        }

        private void SyncEntity<T>(string url, Action<T> action)
        {
            ExecuteRequest(url, x => DeserializeEntity<T>(x, action));
        }

        private void SyncEntitiesArray<TEntity>(string entitiesUrl, Action<TEntity> postSyncAction, bool traceContent = true, bool traceJson = false) where TEntity : class, IEntity
        {
            ExecuteRequest(entitiesUrl, contentStr => DeserializeArray(contentStr, postSyncAction, traceJson), traceContent);
        }

        private void SyncEntitiesArray<TEntity>(string entitiesUrl, bool traceContent = true, bool traceJson = false) where TEntity : class, IEntity
        {
            ExecuteRequest(entitiesUrl, contentStr => DeserializeArray<TEntity>(contentStr, null, traceJson), traceContent);
        }

        private void ExecuteRequest(string relativeUrl, Action<string> successResponseHandler, bool traceRequest = false)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            var proxyServer = LocalConfiguration.GetSetting("ProxyServer");
            if (!string.IsNullOrEmpty(proxyServer))
            {
                clientHandler.Proxy = new WebProxy(proxyServer);
                clientHandler.UseProxy = true;
            }

            client = new HttpClient(clientHandler);

            var queryString = string.Format("{0}{1}", syncServerAddress, relativeUrl);
            var request = new HttpRequestMessage(HttpMethod.Get, queryString);

            AddAuthHeaders(request);

            logger.DebugFormat("SEND: {0}", request.RequestUri);
            var response = client.SendAsync(request).Result;

            var contentStr = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                logger.ErrorFormat("RCVD: {0} - {1}", response.StatusCode, contentStr);
                return;
            }

            if (traceRequest)
            {
                logger.DebugFormat("RCVD: {0} - {1}", response.StatusCode, contentStr);
            }
            else
            {
                var contentBytes = response.Content.ReadAsByteArrayAsync().Result;
                logger.DebugFormat("RCVD: {0} - {1} bytes", response.StatusCode, contentBytes.Length);
            }

            successResponseHandler(contentStr);
        }

        private T DeserializeEntity<T>(string objectJson, Action<T> action)
        {
            var result = JsonConvert.DeserializeObject<T>(objectJson);

            if (action != null)
            {
                action(result);
            }

            return result;
        }

        private TEntity DeserializeEntity<TEntity>(string objectJson, Action<TEntity> postSyncAction, bool trace = false) where TEntity : class, IEntity
        {
            var settings = new JsonSerializerSettings();
            var objectResult = JsonConvert.DeserializeObject<TEntity>(objectJson, settings);
            logger.DebugFormat("Deserialized {0}", objectResult);
            var syncedEntity = EntityMagicSync.Sync(objectResult);

            if (postSyncAction != null)
            {
                postSyncAction((TEntity)syncedEntity);
            }

            return objectResult;
        }

        private void DeserializeArray<TEntity>(string objectJson, Action<TEntity> postSyncAction, bool trace = false) where TEntity : class, IEntity
        {
            var settings = new JsonSerializerSettings();
            //if (trace)
            //{
            //    settings.TraceWriter = new JsonTraceWriter();
            //}

            var remoteEntities = JsonConvert.DeserializeObject<TEntity[]>(objectJson, settings);
            foreach (TEntity remoteEntity in remoteEntities)
            {
                logger.DebugFormat("Deserialized {0}", remoteEntity);
                var syncedEntity = EntityMagicSync.Sync(remoteEntity);
                if (postSyncAction != null)
                {
                    postSyncAction((TEntity)syncedEntity);
                }

                //Console.WriteLine("NEXT ?");
                //Console.ReadLine();
            }
        }

        private void DownloadContentFile(ContentFile contentFile)
        {
            if (contentFile == null || string.IsNullOrEmpty(contentFile.ContentDownloadUrl))
            {
                return;
            }

            if (ContentFileHelper.CheckFileExistence(contentFile))
            {
                logger.DebugFormat("{0} alreday exists", contentFile);
                return;
            }

            var fileRequest = new HttpRequestMessage(HttpMethod.Get, string.Format("{0}/{1}", syncServerAddress, contentFile.ContentDownloadUrl));
            logger.DebugFormat("SEND: {0}", fileRequest.RequestUri);
            var response = client.SendAsync(fileRequest).Result;

            var contentBytes = response.Content.ReadAsByteArrayAsync().Result;
            logger.DebugFormat("RCVD: {0} - {1} bytes", response.StatusCode, contentBytes.Length);
            if (response.IsSuccessStatusCode)
            {
                ContentStorage.Put(contentFile, contentBytes);
            }
        }

        #endregion

        #region Sync methods

        public void SyncCompany()
        {
            SyncEntity<Company>("/api/dictionaries/company", false, false);
        }

        public void SyncUnits()
        {
            SyncEntitiesArray<Unit>("/api/dictionaries/units", false, false);
        }

        public void SyncUnitsWithQueueServiceBindings()
        {
            SyncEntitiesArray<Unit>("/api/dictionaries/units?attachServiceBindings=true", false, false);
        }

        public void SyncQueueSettings()
        {
            SyncEntitiesArray<Unit>("/api/dictionaries/queuesettings", true, true);
        }

        public void SyncRoles()
        {
            SyncEntitiesArray<Role>("/api/dictionaries/roles", false);
        }

        public void SyncUsers()
        {
            SyncEntitiesArray<User>("/api/dictionaries/users", false);
        }

        public void SyncEmployees()
        {
            SyncEntitiesArray<Employee>("/api/dictionaries/employees",
                employee => DownloadContentFile(employee.EmployeePhoto), false);
        }

        public void SyncStyles()
        {
            SyncEntitiesArray<StyleInfo>("/api/dictionaries/styles", false);
        }

        public void SyncSurveys()
        {
            SyncEntitiesArray<Survey>("/api/dictionaries/surveys",
                delegate (Survey survey)
                {
                    foreach (var question in survey.Content.Questions)
                    {
                        DownloadContentFile(question.Picture);
                        DownloadContentFile(question.Video);
                        DownloadContentFile(question.Audio);
                    }

                    if (survey.ScreenplayParts != null)
                    {
                        foreach (var part in survey.ScreenplayParts)
                        {
                            part.DenormalizeSurvey();
                            ScreenplayPartRepository.Save(part);
                        }
                    }
                }, false);
        }

        public void SyncSurveys(int id)
        {
            SyncEntity<Survey>("/api/dictionaries/surveys/" + id,
                delegate (Survey survey)
                {
                    foreach (var question in survey.Content.Questions)
                    {
                        DownloadContentFile(question.Picture);
                        DownloadContentFile(question.Video);
                        DownloadContentFile(question.Audio);
                    }

                    foreach (var part in survey.ScreenplayParts)
                    {
                        part.DenormalizeSurvey();
                        ScreenplayPartRepository.Save(part);
                    }
                }, true, false);
        }

        public void SyncScreenplays()
        {
            SyncEntitiesArray<Screenplay>("/api/dictionaries/screenplays", false);
        }

        public void SyncGroups()
        {
            SyncEntitiesArray<Group>("/api/dictionaries/groups",
                delegate (Group @group)
                {
                    DownloadContentFile(@group.SoftwareUpdate);

                    @group.DenormalizeSoftwareUpdate();
                    GroupRepository.Save(@group);
                }, false);
        }

        public void SyncDevices()
        {
            SyncEntitiesArray<Device>("/api/dictionaries/devices",
                delegate (Device device)
                {
                    if (!device.IsActivated && string.IsNullOrEmpty(device.SecurityKey))
                    {
                        device.SecurityKey = DeviceKeyGenerator.CreateSecurityKey(key => DeviceRepository.Get(key) == null);
                        DeviceRepository.Save(device);
                    }
                }, false);
        }

        public void SyncQueueServiceGroups()
        {
            SyncEntitiesArray<QueueServiceGroup>("/api/dictionaries/queueservicegroups", false);
        }

        public void SyncQueueServices()
        {
            SyncEntitiesArray<QueueService>("/api/dictionaries/queueservices", false);
        }

        public void SyncQueueServicesBindings()
        {
            SyncEntitiesArray<QueueServiceBinding>("/api/dictionaries/queueservicebindings", false);
        }

        public void SyncQueueServiceRules()
        {
            SyncEntitiesArray<QueueServiceRule>("/api/dictionaries/queueservicerules", false);
        }

        public void SyncReportGroups()
        {
            SyncEntitiesArray<ReportGroup>("/api/dictionaries/reportgroups", false);
        }

        public void SyncReports(int id)
        {
            SyncEntity<ReportInfo>("/api/dictionaries/reports/" + id);
        }

        public void SyncReports()
        {
            SyncEntitiesArray<ReportInfo>("/api/dictionaries/reports", false);
        }

        public void SyncDateTime(int unitId)
        {
            SyncEntity<DateTime>("/api/dictionaries/getlocaldatetimeutc?unitId=" + unitId, (x) =>
            {
                logger.DebugFormat("Local date time (UTC) = {0}", x);

                var systemTime = new SYSTEMTIME()
                {
                    wYear = (ushort)x.Year,
                    wMonth = (ushort)x.Month,
                    wDay = (ushort)x.Day,
                    wDayOfWeek = (ushort)x.DayOfWeek,
                    wHour = (ushort)x.Hour,
                    wMinute = (ushort)x.Minute,
                    wSecond = (ushort)x.Second,
                    wMilliseconds = (ushort)x.Millisecond
                };

                var result = SetSystemTime(ref systemTime);

                logger.Debug("Set system time has been " + (result ? "successful" : "failed"));
            });
        }

        #endregion
    }
}