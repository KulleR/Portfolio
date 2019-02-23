using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surveys.Service.Core.Services
{
    /// <summary>
    /// Сервис оповещения администратора
    /// </summary>
    public interface IAdministratorNotificationService
    {
        /// <summary>
        /// Запуск сервиса.
        /// </summary>
        void Start();

        /// <summary>
        /// Остановка сервиса.
        /// </summary>
        void Stop();
    }
}
