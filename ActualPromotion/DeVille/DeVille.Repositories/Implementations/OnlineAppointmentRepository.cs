using Deville.EntityDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.Repositories
{
    public class OnlineAppointmentRepository : IOnlineAppointmentRepository
    {
        private IRepository<OnlineAppointment> onlineAppointmentRepository;

        public OnlineAppointmentRepository(IRepository<OnlineAppointment> _onlineAppointmentRepository)
        {
            this.onlineAppointmentRepository = _onlineAppointmentRepository;
        }

        public IQueryable<OnlineAppointment> GetAll()
        {
            return onlineAppointmentRepository.GetAll();
        }

        public OnlineAppointment GetById(int id)
        {
            if (id == 0)
                return null;
            return onlineAppointmentRepository.GetById(id);
        }

        public int Insert(OnlineAppointment model)
        {
            if (model == null)
                throw new ArgumentNullException("onlineAppointment");
            return onlineAppointmentRepository.Insert(model);
        }

        public void Update(OnlineAppointment model)
        {
            if (model == null)
                throw new ArgumentNullException("onlineAppointment");
            onlineAppointmentRepository.Update(model);

        }

        public void Delete(OnlineAppointment model)
        {
            if (model == null)
                throw new ArgumentNullException("onlineAppointment");
            onlineAppointmentRepository.Delete(model);
        }

        public void SaveChanges()
        {
            onlineAppointmentRepository.SaveChanges();
        }
    }
}
