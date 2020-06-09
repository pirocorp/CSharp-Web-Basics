namespace ModePanel.App.Controllers
{
    using Data.Models;
    using Models.Posts;
    using Services.Contracts;
    using SimpleMvc.Framework.Attributes.Methods;
    using SimpleMvc.Framework.Contracts;

    public class PostsController : BaseController
    {
        private const string CREATE_ERROR = "<p>Check your form for error</p><p>Title – must begin with uppercase letter and has length between 3 and 100 symbols </p><p>Content – must be at least 20 symbols </p>";

        private readonly IPostService _postService;

        public PostsController(ILogService logService, IPostService postService, 
            IUserService userService) 
            : base(logService, userService)
        {
            this._postService = postService;
        }

        public IActionResult Create()
        {
            if (!this.User.IsAuthenticated)
            {
                return this.RedirectToLogin();
            }

            return this.View();
        }

        [HttpPost]
        public IActionResult Create(PostModel model)
        {
            if (!this.User.IsAuthenticated)
            {
                return this.RedirectToLogin();
            }

            if (!this.IsValidModel(model))
            {
                this.ShowError(CREATE_ERROR);
                return this.View();
            }

            this._postService
                .Create(model.Title, 
                    model.Content,
                    this.Profile.Id);

            if (this.IsAdmin)
            {
                this.Log(LogType.CreatePost, model.Title);
            }

            return this.RedirectToHome();
        }
    }
}
