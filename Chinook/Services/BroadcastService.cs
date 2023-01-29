using Chinook.Services.Interfaces;

namespace Chinook.Services
{
    public class BroadcastService : IBroadcastService
    {
        public event EventHandler? NavLinkUpdateCallback;

        public void RegisterNavLinkUpdateCallback(EventHandler callback)
        {
            NavLinkUpdateCallback += callback;
        }

        public void TriggerNavRefresh()
        {
            NavLinkUpdateCallback?.Invoke(this, EventArgs.Empty);
        }
    }
}
