namespace Surveys.Service.Core.Sync.Components.SignalR
{
    public interface ISignalRHost
    {
        bool Start();

        void Stop();
    }
}
