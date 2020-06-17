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
                Albums = this._albumsService
                    .GetAll(a => new AlbumInfoModel()
                        {
                            Id = a.Id,
                            Name = a.Name
                        }),
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

            var viewModel = this._albumsService
                .GetDetails(id, x => new AlbumDetailsModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    Cover = x.Cover,
                    Tracks = x.Tracks
                        .Select(t => new TrackAlbumDetailsViewModel()
                        {
                            Id = t.Id,
                            Name = t.Name
                        })
                });

            return this.View(viewModel);
        }
    }
}
