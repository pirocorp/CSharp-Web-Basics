namespace IRunes.App.Controllers
{
    using Services;
    using SIS.HTTP;
    using SIS.MvcFramework;
    using ViewModels.Tracks;

    public class TracksController : Controller
    {
        private readonly ITracksService _tracksService;

        public TracksController(ITracksService tracksService)
        {
            this._tracksService = tracksService;
        }

        public HttpResponse Create(string albumId)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var viewModel = new CreateViewModel()
            {
                AlbumId = albumId,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public HttpResponse Create(CreateTrackInputModel input, string albumId)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (input.Name.Length < 4
                || input.Name.Length > 20)
            {
                return this.Error("Track name should be between 4 and 20 characters.");
            }

            if(!input.Link.StartsWith("https://")
            && !input.Link.StartsWith("http://"))
            {
                return this.Error("Invalid Link.");
            }

            if (input.Price < 0)
            {
                return this.Error("Price should be a positive number.");
            }

            this._tracksService.Create(input.Name, input.Link, input.Price, albumId);
            return this.Redirect($"/Albums/Details?id={albumId}");
        }

        public HttpResponse Details(string trackId)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var viewModel = this._tracksService
                .GetDetails(trackId, x => new TrackDetailsModel()
                {
                    AlbumId = x.AlbumId,
                    Link = x.Link,
                    Name = x.Name,
                    Price = x.Price
                });

            if (viewModel == null)
            {
                return this.Redirect("/Albums/All");
            }

            return this.View(viewModel);
        }
    }
}
