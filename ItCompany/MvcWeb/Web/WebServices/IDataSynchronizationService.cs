using System.Collections.Generic;
using System.ServiceModel;
using Surveys.Core.Model.DTO.Sync;

namespace Surveys.Web.Host.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "DataSynchronizationService" in both code and config file together.
    [ServiceContract]
    public interface IDataSynchronizationService
    {
        [OperationContract]
        void DeviceSynchronize(List<DeviceSyncDto> devices);
        
        [OperationContract]
        void DeviceActivitiesSynchronize(List<DeviceStateActivitySyncDto> deviceActivities);

        [OperationContract]
        void QueueTicketSynchronize(List<QueueTicketSyncDto> queueTickets);

        [OperationContract]
        void ConversationSynchronize(List<ConversationSyncDto> conversations);

        [OperationContract]
        void SessionSynchronize(List<SessionSyncDto> sessions);

        [OperationContract]
        void TestSessionSynchronize(List<TestSessionSyncDto> testSessions);

        [OperationContract]
        void UnitEventsSynchronize(List<UnitEventDto> unitEvents);
    }
}
