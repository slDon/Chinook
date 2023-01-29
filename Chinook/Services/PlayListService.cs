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

        public async Task<IList<ClientModels.Playlist>> GetPlayListsByUserIdAsync(string userId)
        {
            var dbContext = await DbFactory.CreateDbContextAsync();
            var playlist = new List<ClientModels.Playlist>();

            playlist = dbContext.Playlists
                .Include(a => a.UserPlaylists)
                .Where(p => p.UserPlaylists.Any(up => up.UserId == userId))
                .Select(p => new ClientModels.Playlist()
                {
                    PlaylistId = p.PlaylistId,
                    Name = p.Name.ToString()
                }).ToList();

            return playlist;
        }
    }
}
