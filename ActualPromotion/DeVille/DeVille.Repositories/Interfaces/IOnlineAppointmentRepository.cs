using Deville.EntityDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.Repositories
{
    public interface IOnlineAppointmentRepository
    {
        IQueryable<OnlineAppointment> GetAll();
        OnlineAppointment GetById(int id);
        int Insert(OnlineAppointment model);
        void Update(OnlineAppointment model);
        void Delete(OnlineAppointment model);
        void SaveChanges();
    }
}
