namespace Chinook.Services.Interfaces
{
    public interface IBroadcastService
    {
        void RegisterNavLinkUpdateCallback(EventHandler callback);
        void TriggerNavRefresh();
    }
}
