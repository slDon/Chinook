using Chinook.Models;

namespace Chinook.Services.Interfaces
{
    public interface IAlbumService
    {
        public Task<IList<Album>> GetAllAlbumsAsync();
        public Task<IList<Album>> GetAlbumsByArtisIdAsync(long artisId);
    }
}
