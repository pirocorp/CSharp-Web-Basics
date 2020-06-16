namespace IRunes.App.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using Services;
    using SIS.HTTP;
    using SIS.MvcFramework;
    using ViewModels.Albums;

    public class AlbumsController : Controller
    {
        private readonly IAlbumsService _albumsService;

        public AlbumsController(IAlbumsService albumsService)
        {
            this._albumsService = albumsService;
        }

        public HttpResponse All()
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var viewModel = new AllAlbumsViewModel()
            {
                Albums = this._albumsService.GetAll(),
            };

            return this.View(viewModel);
        }

        public HttpResponse Create()
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Create(CreateInputModel input)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (input.Name.Length < 4
                || input.Name.Length > 20)
            {
                return this.Error("Name length is between 4 and 20 symbols.");
            }

            if (string.IsNullOrWhiteSpace(input.Cover))
            {
                return this.Error("Cover is required");
            }

            this._albumsService.Create(input.Name, input.Cover);
            return this.Redirect("/Albums/All");
        }

        public HttpResponse Details(string id)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var serviceModel = this._albumsService.GetDetails(id);

            var viewModel = new AlbumDetailsViewModel()
            {
                Id = serviceModel.Id,
                Name = serviceModel.Name,
                Cover = serviceModel.Cover,
                Price = serviceModel.Price,
                Tracks = serviceModel.Tracks
                    .Select(t => new TrackAlbumDetailsModel()
                    {
                        Id = t.Id,
                        Name = t.Name
                    }).ToList()
            };

            return this.View(viewModel);
        }
    }
}
