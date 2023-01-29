using Chinook.Models;

namespace Chinook.Services.Interfaces
{
    public interface IArtistService
    {
        public Task<IList<Artist>> GetAllArtistsAsync();
        public Task<Artist?> GetArtistByArtisIdAsync(long artisId);
    }
}
