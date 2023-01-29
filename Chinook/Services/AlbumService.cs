using Chinook.Models;
using Chinook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Services
{
    public class AlbumService : IAlbumService
    {
        IDbContextFactory<ChinookContext> DbFactory { get; set; }

        public AlbumService(IDbContextFactory<ChinookContext> dbContextFactory)
        {
            this.DbFactory = dbContextFactory;
        }

        public async Task<IList<Album>> GetAllAlbumsAsync()
        {
            var DbContext = await DbFactory.CreateDbContextAsync();

            return await DbContext.Albums.ToListAsync();
        }

        public async Task<IList<Album>> GetAlbumsByArtisIdAsync(long artisId)
        {
            var DbContext = await DbFactory.CreateDbContextAsync();

            return await DbContext.Albums.Where(a => a.ArtistId == artisId).ToListAsync();
        }
    }
}
