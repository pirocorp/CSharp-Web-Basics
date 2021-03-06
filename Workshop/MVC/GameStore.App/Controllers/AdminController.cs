﻿namespace GameStore.App.Controllers
{
    using System.Linq;
    using Data.Models;
    using Infrastructure;
    using Models.Games;
    using Services.Contracts;
    using SimpleMvc.Framework.Attributes.Methods;
    using SimpleMvc.Framework.Contracts;

    public class AdminController : BaseController
    {
        private const string GAME_ERROR = @"<p>Title – has to begin with uppercase letter and has length between 3 and 100 symbols (inclusive)</p><p>Price –  must be a positive number with precision up to 2 digits after floating point</p><p>Size – must be a positive number with precision up to 1 digit after floating point</p><p>Trailer– only videos from YouTube are allowed and only their ID should be saved to the database which is a string of exactly 11 characters.</p><p>Thumbnail URL – it should be a plain text starting with http://, https:// or null</p><p>Description – must be at least 20 symbols</p>";

        private readonly IGamesService _gamesService;

        public AdminController(IGamesService gamesService)
        {
            this._gamesService = gamesService;
        }

        public IActionResult AllGames()
        {
            if (!this.IsAdmin)
            {
                return this.RedirectToHome();
            }

            var games = this._gamesService.All<GameListingAdminModel>()
                .Select(g => g.ToHtml());

            this.ViewModel["games"] = string.Join(string.Empty, games);
            return this.View();
        }

        public IActionResult AddGame()
        {
            if (!this.IsAdmin)
            {
                return this.RedirectToHome();
            }

            return this.View();
        }

        [HttpPost]
        public IActionResult AddGame(GameAdminModel model)
        {
            if (!this.IsAdmin)
            {
                return this.RedirectToHome();
            }

            if (!this.IsValidModel(model))
            {
                this.ShowError(GAME_ERROR);
                return this.View();
            }

            this._gamesService.Create(
                model.Title, model.Description, model.ThumbnailUrl, model.Price,
                model.Size, model.VideoId, model.ReleaseDate);

            return this.RedirectToAllGames();
        }

        public IActionResult EditGame(int id)
        {
            if (!this.IsAdmin)
            {
                return this.RedirectToHome();
            }

            var game = this._gamesService.GetById<GameDetailsModel>(id);

            if (game == null)
            {
                return this.NotFound();
            }

            this.SetGameViewData(game);
            return this.View();
        }

        [HttpPost]
        public IActionResult EditGame(int id, GameAdminModel model)
        {
            if (!this.IsAdmin)
            {
                return this.RedirectToHome();
            }

            if (!this.IsValidModel(model))
            {
                this.ShowError(GAME_ERROR);
                return this.View();
            }

            this._gamesService.Update(id, model.Title, model.Description, 
                model.ThumbnailUrl, model.Price, model.Size, model.VideoId, 
                model.ReleaseDate);

            return this.RedirectToAllGames();
        }

        public IActionResult DeleteGame(int id)
        {
            if (!this.IsAdmin)
            {
                return this.RedirectToHome();
            }

            var game = this._gamesService.GetById<GameDetailsModel>(id);

            if (game == null)
            {
                return this.NotFound();
            }

            this.ViewModel["id"] = id.ToString();

            this.SetGameViewData(game);
            return this.View();
        }

        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            if (!this.IsAdmin)
            {
                return this.RedirectToHome();
            }

            var game = this._gamesService.GetById<GameDetailsModel>(id);

            if (game == null)
            {
                return this.NotFound();
            }

            this._gamesService.Delete(id);
            return this.RedirectToAllGames();
        }

        private void SetGameViewData(GameDetailsModel game)
        {
            this.ViewModel["title"] = game.Title;
            this.ViewModel["description"] = game.Description;
            this.ViewModel["thumbnail"] = game.ThumbnailUrl;
            this.ViewModel["price"] = game.Price.ToString("F2");
            this.ViewModel["size"] = game.Size.ToString("F1");
            this.ViewModel["videoId"] = game.VideoId;
            this.ViewModel["releaseDate"] = game.ReleaseDate.ToString("yyyy-MM-dd");
        }

        private IActionResult RedirectToAllGames()
            => this.Redirect("/admin/allGames");
    }
}
