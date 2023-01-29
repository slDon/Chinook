using Chinook.Models;
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

        public IList<ClientModels.PlaylistTrack> GetTracksByArtistIdAndUserId(long artistId, string userId)
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

        public async Task<ClientModels.PlaylistTrack> AddTrackToPlayListAsync(long playListId, long trackId)
        {
            ClientModels.PlaylistTrack result = new ClientModels.PlaylistTrack();
            Track track = new Track();
            var DbContext = await DbFactory.CreateDbContextAsync();

            var record = DbContext.PlaylistTracks.Where(a => a.TrackId == trackId && a.PlaylistId == playListId).FirstOrDefault();
            track = DbContext.Tracks
                .Include(a => a.Album).ThenInclude(a => a.Artist)
                .Where(a => a.TrackId == trackId).FirstOrDefault();

            if (track != null && record == null)
            {
                DbContext.PlaylistTracks.Add(new Models.PlaylistTrack
                {
                    TrackId = trackId,
                    PlaylistId = playListId
                });
                DbContext.SaveChanges();

                result = DbContext.Tracks
                    .Include(a => a.Playlists)
                    .Include(a => a.Album).ThenInclude(a => a.Artist)
                    .Where(a => a.TrackId == trackId)
                    .Select(p => new ClientModels.PlaylistTrack()
                    {
                        AlbumTitle = p.Album.Title,
                        ArtistName = p.Album.Artist.Name,
                        TrackId = p.TrackId,
                        TrackName = p.Name,
                        IsFavorite = p.Playlists.Where(p => p.UserPlaylists.Any(up => up.PlaylistId == playListId && up.Playlist.Name == "My favorite tracks")).Any()
                    })
                    .FirstOrDefault();

            }

            return result;
        }

        public async Task<bool> RemoveTrackFromPlayListAsync(long playListId, long trackId)
        {
            bool result = false;
            var DbContext = await DbFactory.CreateDbContextAsync();


            var record = DbContext.PlaylistTracks.Where(a => a.TrackId == trackId && a.PlaylistId == playListId).FirstOrDefault();
            if (record != null)
            {
                DbContext.PlaylistTracks
                .Remove(record);
                DbContext.SaveChanges();
                result = true;
            }

            return result;
        }
    }
}
