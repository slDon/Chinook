using Chinook.ClientModels;

namespace Chinook.Services.Interfaces
{
    public interface ITrackService
    {
        public IList<ClientModels.PlaylistTrack> GetTracksByArtistIdAndUserId(long artistId, string userId);
        public Task<PlaylistTrack> AddTrackToPlayListAsync(long playListId, long trackId);
        public Task<bool> RemoveTrackFromPlayListAsync(long playListId, long trackId);
    }
}
