using Chinook.ClientModels;
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

        public IList<PlaylistTrack> GetTracksByArtistIdAndUserId(long artistId, string userId)
        {
            var DbContext = DbFactory.CreateDbContext();

            return DbContext.Tracks.Where(a => a.Album.ArtistId == artistId)
            .Include(a => a.Album)
            .Select(t => new ClientModels.PlaylistTrack()
            {
                AlbumTitle = (t.Album == null ? "-" : t.Album.Title),
                TrackId = t.TrackId,
                TrackName = t.Name,
                IsFavorite = t.Playlists.Where(p => p.UserPlaylists.Any(up => up.UserId == userId && up.Playlist.Name == "Favorites")).Any()
            })
            .ToList();
        }
    }
}
