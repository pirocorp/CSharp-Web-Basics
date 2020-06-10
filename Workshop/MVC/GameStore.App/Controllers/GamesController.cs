namespace GameStore.App.Controllers
{
    using AutoMapper;
    using Infrastructure;
    using Models.Games;
    using Services.Contracts;
    using SimpleMvc.Framework.Contracts;

    public class GamesController : BaseController
    {
        private readonly IGamesService _gamesService;

        public GamesController(IGamesService gamesService)
        {
            this._gamesService = gamesService;
        }

        public IActionResult Details(int id)
        {
            var game = this._gamesService
                .GetById<GameDetailsModel>(id)
                .ToHtml(this.IsAdmin);

            this.ViewModel["gameDetails"] = game;

            return this.View();
        }
    }
}
