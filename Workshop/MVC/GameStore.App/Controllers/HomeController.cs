namespace GameStore.App.Controllers
{
    using System.Linq;
    using System.Text;
    using AutoMapper;
    using Infrastructure;
    using Models.Home;
    using Services.Contracts;
    using SimpleMvc.Framework.Contracts;

    public class HomeController : BaseController
    {
        private readonly IGamesService _gamesService;
        private readonly IMapper _mapper;

        public HomeController(IGamesService gamesService, IMapper mapper)
        {
            this._gamesService = gamesService;
            this._mapper = mapper;
        }

        public IActionResult Index()
        {
            var ownedGames = this.User.IsAuthenticated
                && this.Request.UrlParameters.ContainsKey("filter")
                && this.Request.UrlParameters["filter"] == "Owned";

            var gameCards = this._gamesService
                .All<GameListingHomeModel>(ownedGames ? (int?)this.Profile.Id : null)
                .Select(g => g.ToHtml(this.IsAdmin))
                .ToList();

            var result = new StringBuilder();

            for (var i = 0; i < gameCards.Count; i++)
            {
                if (i % 3 == 0)
                {
                    if (i != 0)
                    {
                        result.Append("</div>");
                    }

                    result.Append(@"<div class=""card-group"">");
                }

                result.Append(gameCards[i]);
            }

            if (gameCards.Count % 3 != 0)
            {
                result.Append("</div>");
            }

            this.ViewModel["games"] = result.ToString();
            
            return this.View();
        }
    }
}
