using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.UI;
using Common.Logging;
using Microsoft.Practices.Unity;
using NHibernate.Engine;
using Surveys.Core.Configuration;
using Surveys.Core.Model;
using Surveys.Core.Model.DTO;
using Surveys.Core.Model.Queue;
using Surveys.Core.Repositories;
using Surveys.Devices.Printers;
using Surveys.Service.Core.Handlers;
using Surveys.Service.Core.Signalr;

namespace Surveys.Service.Core.Services
{
    public class AdministratorNotificationService : IAdministratorNotificationService
    {
        #region Dependency

        [Dependency]
        public IDeviceStateActivityRepository DeviceStateActivityRepository { get; set; }

        [Dependency]
        public IUnitRepository UnitRepository { get; set; }

        [Dependency]
        public IDeviceRepository DeviceRepository { get; set; }

        [Dependency]
        public IQueueServiceRepository QueueServiceRepository { get; set; }

        [Dependency]
        public DashboardBroadcastService DashboardBroadcastService { get; set; }

        /// <summary>
        /// Статус принтера. Получается от принтера.
        /// </summary>
        public static string PrinterStatus { get; set; }

        [Dependency]
        public EQueueHandler EQueueHandler { get; set; }

        #endregion

        #region Private fields

        /// <summary>
        /// Логировщик.
        /// </summary>
        private static readonly ILog logger = LogManager.GetLogger<AdministratorNotificationService>();

        /// <summary>
        /// Таймер опроса продолжительности перерыва.
        /// </summary>
        private Timer _breakDurationCheckTimer;

        /// <summary>
        /// Таймер опроса .
        /// </summary>
        private Timer _activeServiceCheckTimer;

        /// <summary>
        /// Таймер опроса статуса принтера.
        /// </summary>
        private Timer _printerStatusCheckTimer;

        /// <summary>
        /// Открыт ли принтер.
        /// </summary>
        private bool _isOpened;

        #endregion

        public void Start()
        {
            logger.Info("Administrator notification service starting");

            try
            {
                var period = (long)TimeSpan.FromMinutes(LocalConfiguration.GetDoubleSetting("AdminNotificationIntervalMin", 10)).TotalMilliseconds;
                _breakDurationCheckTimer = new Timer(CheckAdminWarningMessages, null, 0, period);
                logger.Info("Administrator notification service started");
            }
            catch (Exception e)
            {
                logger.ErrorFormat("Unable to start administrator notification service", e);
            }
        }

        public void Stop()
        {
            logger.Info("Stopping Administrator notification service service!");
            _breakDurationCheckTimer.Dispose();
        }

        #region Timer callbacks


        private void CheckAdminWarningMessages(object o)
        {
            var exceedingDevices = new List<Device>();
            var unavailibleServices = new List<QueueService>();

            try
            {
                DeviceRepository.Wrap(() =>
                {
                    var unit = UnitRepository.GetAll().FirstOrDefault();
                    exceedingDevices = GetBreakExceedingDevices(unit);
                    unavailibleServices = GetUnavailibleServices(unit);
                });
            }
            catch (Exception e)
            {
                logger.Error(e);
            }

            var notifications = new List<AdminNotificationDto>();
            if (exceedingDevices.Any())
            {
                notifications.Add(new AdminNotificationDto
                {
                    Message = "Превышено время перерыва у окон:",
                    Description = exceedingDevices.Aggregate(new StringBuilder(), (builder, device) => builder.AppendLine($"{device.Description}")).ToString()
                });
            }

            if (PrinterStatus == "PaperEnd")
            {
                notifications.Add(new AdminNotificationDto
                {
                    Message = "Закончилась бумага в принтере."
                });
            }

            if (unavailibleServices.Any())
            {
                notifications.Add(new AdminNotificationDto
                {
                    Message = "Дальнейшее оказание следующих услуг невозможно ни в одном окне:",
                    Description = unavailibleServices.Aggregate(new StringBuilder(), (builder, service) => builder.AppendLine($"({service.Prefix}) {service.DisplayName}")).ToString()
                });
            }

            DashboardBroadcastService.NotifyAdmin(notifications);
        }


        #endregion


        private List<Device> GetBreakExceedingDevices(Unit unit)
        {
            List<Device> devices = new List<Device>();

            var maxAllowedBreakDuration = unit?.QueueSetting?.MaxAllowedBreakDuration;
            if (maxAllowedBreakDuration.HasValue && maxAllowedBreakDuration.Value > 0)
            {
                DateTime oldestDate = DateTime.Now.Subtract(TimeSpan.FromMinutes(maxAllowedBreakDuration.Value));

                devices =
                    DeviceStateActivityRepository.GetAll()
                        .Where(
                            a =>
                                !a.Closed && a.EQueueDeviceState == DeviceState.Pause &&
                                a.StartDate <= oldestDate).Select(a => a.Device).ToList();
            }

            return devices;
        }

        private List<QueueService> GetUnavailibleServices(Unit unit)
        {
            try
            {
                var allServicesIds = DeviceRepository.Query().
                    FirstOrDefault(d => d.Type == DeviceType.EqueueRegistration)?.
                    Survey?.Content.OrderedQuestionsResponses.
                    Where(r => r.Value > 0).Select(r => r.Value);

                if (allServicesIds == null || !allServicesIds.Any())
                {
                    return new List<QueueService>();
                }

                var allServices = QueueServiceRepository.Query().Where(s => allServicesIds.Contains(s.Id));

                var online = EQueueHandler.GetOnlineDevices(unit);
                var availibleServices = online.SelectMany(d => d.BindedQueueServices).Select(s => s.Id).Distinct();

                return
                    allServices.Where(s => !availibleServices.Contains(s.Id))
                        .OrderBy(qs => qs.Prefix)
                        .ThenBy(qs => qs.DisplayName)
                        .ToList();
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("Unable to get unavailible services", ex);
                return Enumerable.Empty<QueueService>().ToList();
            }
        }
    }
}
