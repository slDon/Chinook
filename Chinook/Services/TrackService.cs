using Chinook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Services
{
    public class TrackService : ITrackService
    {
        IDbContextFactory<ChinookContext> DbFactory { get; set; }

        public TrackService(IDbContextFactory<ChinookContext> dbContextFactory)
        {
            this.DbFactory = dbContextFactory;
        }
    }
}
