namespace Chinook.Services.Interfaces
{
    public interface ITrackService
    {
        public IList<ClientModels.PlaylistTrack> GetTracksByArtistIdAndUserId(long artistId, string userId);
    }
}
