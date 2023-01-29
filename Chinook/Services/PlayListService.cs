using Chinook.Models;
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

        public async Task<ClientModels.Playlist?> GetPlayListByNameAsync(string userId, string playlistName)
        {
            var dbContext = await DbFactory.CreateDbContextAsync();
            ClientModels.Playlist? playlist = null;

            var record = dbContext.Playlists
                .Where(p => p.Name == playlistName)
                .FirstOrDefault();

            if (record != null)
            {
                playlist = new ClientModels.Playlist()
                {
                    Name = record.Name,
                    PlaylistId = record.PlaylistId
                };
            }

            return playlist;
        }

        public async Task<ClientModels.Playlist> CreatePlayListAsync(string userId, string newPlaylistName)
        {
            var dbContext = await DbFactory.CreateDbContextAsync();
            var playlist = new ClientModels.Playlist();

            var record = dbContext.Playlists
                .Where(p => p.Name == newPlaylistName)
                .FirstOrDefault();

            if (record == null)
            {
                long nextId = dbContext.Playlists.OrderByDescending(a => a.PlaylistId).AsNoTracking().FirstOrDefault().PlaylistId + 1;
                Models.Playlist newPlaylist = new Models.Playlist()
                {
                    PlaylistId = nextId,
                    Name = newPlaylistName
                };
                await dbContext.Playlists.AddAsync(newPlaylist);
                dbContext.SaveChanges();

                Models.UserPlaylist userPlaylist = new UserPlaylist()
                {
                    PlaylistId = newPlaylist.PlaylistId,
                    UserId = userId
                };
                await dbContext.UserPlaylists.AddAsync(userPlaylist);
                dbContext.SaveChanges();

                playlist.PlaylistId = newPlaylist.PlaylistId;
                playlist.Name = newPlaylist.Name;
            }


            return playlist;
        }

        public async Task<ClientModels.Playlist> GetPlayListByUserIdAndPlayListIdAsync(string userId, long playListId)
        {
            var dbContext = await DbFactory.CreateDbContextAsync();
            var playlist = new ClientModels.Playlist();

            playlist = dbContext.Playlists
                .Include(a => a.UserPlaylists)
                .Include(a => a.PlaylistTracks).ThenInclude(b => b.Track).ThenInclude(a => a.Album).ThenInclude(a => a.Artist)
                .Where(p => p.PlaylistId == playListId && p.UserPlaylists.Any(a => a.UserId == userId && a.PlaylistId == playListId))
                .Select(p => new ClientModels.Playlist()
                {
                    PlaylistId = p.PlaylistId,
                    Name = p.Name,
                    Tracks = p.PlaylistTracks.Select(t => new ClientModels.PlaylistTrack()
                    {
                        AlbumTitle = t.Track.Album.Title,
                        ArtistName = t.Track.Album.Artist.Name,
                        TrackId = t.Track.TrackId,
                        TrackName = t.Track.Name,
                        IsFavorite = t.Track.Playlists.Where(p => p.UserPlaylists.Any(up => up.UserId == userId && up.Playlist.Name == "My favorite tracks")).Any()
                    }).ToList()
                })
                .FirstOrDefault();

            return playlist;
        }

        public async Task<ClientModels.Playlist> RenamePlayListByIdAsync(long playListId, string newPlaylistName)
        {
            var dbContext = await DbFactory.CreateDbContextAsync();
            var playlist = new ClientModels.Playlist();

            var record = dbContext.Playlists
                .Where(p => p.PlaylistId == playListId)
                .FirstOrDefault();

            if (record != null)
            {
                record.Name = newPlaylistName;
                dbContext.Update(record);
                dbContext.SaveChanges();

                playlist.PlaylistId = record.PlaylistId;
                playlist.Name = record.Name;
            }


            return playlist;
        }
    }
}
