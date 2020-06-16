
namespace IRunes.Services
{
    using Models.Tracks;

    public interface ITracksService
    {
        void Create(string name, string link, decimal price, string albumId);

        TrackDetailsServiceModel GetDetails(string trackId);
    }
}
