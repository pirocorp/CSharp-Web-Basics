namespace MyCoolWebServer.GameStoreApplication.Controllers
{
    using System;
    using System.Linq;
    using Server.Http.Contracts;
    using Services;
    using Services.Contracts;
    using ViewModels.Admin;

    public class AdminController : BaseController
    {
        private const string ADD_GAME_VIEW = "./Admin/Add-Game";
        private const string GAMES_LIST = "./Admin/Admin-Games";

        private readonly IGameService _gameService;

        public AdminController(IHttpRequest request) 
            : base(request)
        {
            this._gameService = new GameService();
        }

        public IHttpResponse Add()
        {
            if (!this.Authentication.IsAdmin)
            {
                return this.RedirectResponse(HomePath);
            }

            return this.FileViewResponse(ADD_GAME_VIEW);
        }

        public IHttpResponse Add(AdminAddGameViewModel model)
        {
            if (!this.Authentication.IsAdmin)
            {
                return this.RedirectResponse(HomePath);
            }

            if (!this.ValidateModel(model))
            {
                return this.Add();
            }

            if (!model.ReleaseDate.HasValue)
            {
                this.ShowError("Missing release date.");
                return this.Add();
            }

            this._gameService.Create(model.Title, model.Description, model.Image,
                model.Price, model.Size, model.VideoId, model.ReleaseDate.Value);

            return this.RedirectResponse("/admin/games/list");
        }

        public IHttpResponse List()
        {
            if (!this.Authentication.IsAdmin)
            {
                return this.RedirectResponse(HomePath);
            }

            var result = this._gameService
                .All()
                .Select((g, i) => @$"<tr class=""{(i % 2 == 0 ? "table-success" : "")}""><th scope=""row"">{g.Id}</th><td>{g.Title}</td><td>{g.Size:F1} GB</td><td>{g.Price:F2} &euro;</td><td><a href=""/admin/games/edit/{g.Id}"" class=""btn btn-success btn-sm"">Edit</a><a href=""/admin/games/delete/{g.Id}"" class=""btn btn-danger btn-sm"">Delete</a></td></tr>")
                .ToArray();

            var resultsAsHtml = string.Join(Environment.NewLine, result);

            this.ViewData["games"] = resultsAsHtml;

            return this.FileViewResponse(GAMES_LIST);
        }
    }
}
