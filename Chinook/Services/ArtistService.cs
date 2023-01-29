using Chinook.Models;
using Chinook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Services
{
    public class ArtistService : IArtistService
    {
        IDbContextFactory<ChinookContext> DbFactory { get; set; }

        public ArtistService(IDbContextFactory<ChinookContext> dbContextFactory)
        {
            this.DbFactory = dbContextFactory;
        }

        public async Task<IList<Artist>> GetAllArtistsAsync()
        {
            var DbContext = await DbFactory.CreateDbContextAsync();

            return await DbContext.Artists.ToListAsync();
        }
    }
}
