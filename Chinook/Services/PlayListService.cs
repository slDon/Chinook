using Chinook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Services
{
    public class PlayListService : IPlayListService
    {
        IDbContextFactory<ChinookContext> DbFactory { get; set; }

        public PlayListService(IDbContextFactory<ChinookContext> dbContextFactory)
        {
            this.DbFactory = dbContextFactory;
        }
    }
}
