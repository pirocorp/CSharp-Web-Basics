
namespace IRunes.Services
{
    using System;
    using IRunes.Models;

    public interface ITracksService
    {
        void Create(string name, string link, decimal price, string albumId);

        T GetDetails<T>(string trackId, Func<Track, T> selectFunc);
    }
}
