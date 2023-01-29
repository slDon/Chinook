namespace Chinook.Services.Interfaces
{
    public interface IPlayListService
    {
        public Task<IList<ClientModels.Playlist>> GetPlayListsByUserIdAsync(string userId);
    }
}
