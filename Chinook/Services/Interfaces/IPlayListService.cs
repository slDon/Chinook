namespace Chinook.Services.Interfaces
{
    public interface IPlayListService
    {
        public Task<IList<ClientModels.Playlist>> GetPlayListsByUserIdAsync(string userId);
        public Task<ClientModels.Playlist?> GetPlayListByNameAsync(string userId, string playlistName);
        public Task<ClientModels.Playlist> CreatePlayListAsync(string userId, string newPlaylistName);
        public Task<ClientModels.Playlist> GetPlayListByUserIdAndPlayListIdAsync(string userId, long playListId);
        public Task<ClientModels.Playlist> RenamePlayListByIdAsync(long playListId, string newPlaylistName);
    }
}
