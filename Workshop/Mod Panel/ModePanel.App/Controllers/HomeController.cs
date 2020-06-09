namespace ModePanel.App.Controllers
{
    using System;
    using System.Linq;
    using Infrastructure;
    using Services.Contracts;
    using SimpleMvc.Framework.Contracts;

    public class HomeController : BaseController
    {
        private readonly IPostService _postService;

        public HomeController(ILogService logService, IPostService postService, 
            IUserService userService) 
            : base(logService, userService)
        {
            this._postService = postService;
        }

        public IActionResult Index()
        {
            this.ViewModel["guestDisplay"] = "block";
            this.ViewModel["authenticatedDisplay"] = "none";
            this.ViewModel["adminDisplay"] = "none";

            if (this.User.IsAuthenticated)
            {
                this.ViewModel["guestDisplay"] = "none";
                this.ViewModel["authenticatedDisplay"] = "flex";

                string query = null;
                if (this.Request.UrlParameters.ContainsKey("query"))
                {
                    query = this.Request.UrlParameters["query"];
                }

                var postsData = this._postService.AllWithData(query);

                var postsCards = postsData
                    .Select(p => $@"
                            <div class=""card border-primary mb-3"">
                                <div class=""card-body text-primary"">
                                    <h4 class=""card-title"">{p.Title}</h4>
                                    <p class=""card-text"">
                                        {p.Content}
                                    </p>
                                </div>
                                <div class=""card-footer bg-transparent text-right"">
                                    <span class=""text-muted"">
                                        Created on {(p.CreatedOn ?? DateTime.UtcNow).ToShortDateString()} by
                                        <em>
                                            <strong>{p.CreatedBy}</strong>
                                        </em>
                                    </span>
                                </div>
                            </div>");

                this.ViewModel["posts"] = postsCards.Any() 
                    ? string.Join(string.Empty, postsCards)
                    : "<h2>No posts found!</h2>";

                if (this.IsAdmin)
                {
                    this.ViewModel["authenticatedDisplay"] = "none";
                    this.ViewModel["adminDisplay"] = "flex";

                    var logsResult = this.logService
                        .All()
                        .Select(l => l.ToHtml());

                    this.ViewModel["logs"] = string.Join(string.Empty, logsResult);
                }
            }

            return this.View();
        }
    }
}
