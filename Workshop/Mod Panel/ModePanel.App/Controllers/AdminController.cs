namespace ModePanel.App.Controllers
{
    using System;
    using System.Linq;
    using Infrastructure;
    using Models.Posts;
    using Services;
    using Services.Contracts;
    using SimpleMvc.Framework.Attributes.Methods;
    using SimpleMvc.Framework.Contracts;

    public class AdminController : BaseController
    {
        private const string EDIT_ERROR = "<p>Check your form for error</p><p>Title – must begin with uppercase letter and has length between 3 and 100 symbols </p><p>Content – must be at least 20 symbols </p>";

        private readonly IUserService _userService;
        private readonly IPostService _postService;

        public AdminController()
        {
            this._userService = new UserService();
            this._postService = new PostService();
        }

        public IActionResult Users()
        {
            if (!this.IsAdmin)
            {
                return this.RedirectToLogin();
            }

            var rows = this._userService
                .All()
                .Select(u => $@"
                    <tr>
                        <td>{u.Id}</td>
                        <td>{u.Email}</td>
                        <td>{u.Position.ToFriendlyName()}</td>
                        <td>{u.Posts}</td>
                        <td>
                            {(u.IsApproved ? string.Empty : $@"<a class=""btn btn-primary btn-sm"" href=""/admin/approve?id={u.Id}"">Approve</a>")}
                        </td>
                    </tr>");

            var result = string.Join(Environment.NewLine, rows);
            this.ViewModel["users"] = result;

            return this.View();
        }

        public IActionResult Approve(int id)
        {
            if (!this.IsAdmin)
            {
                return this.RedirectToLogin();
            }

            this._userService.Approve(id);
            return this.Redirect("/admin/users");
        }

        public IActionResult Posts()
        {
            if (!this.IsAdmin)
            {
                return this.RedirectToLogin();
            }

            var rows = this._postService
                .All()
                .Select(p => $@"
                    <tr>
                        <td>{p.Id}</td>
                        <td>{p.Title}</td>
                        <td>
                            <a class=""btn btn-warning btn-sm"" href=""/admin/edit?id={p.Id}"">Edit</a>
                            <a class=""btn btn-danger btn-sm"" href=""/admin/delete?id={p.Id}"">Delete</a>
                        </td>
                    </tr>");
            
            var result = string.Join(Environment.NewLine, rows);
            this.ViewModel["posts"] = result;

            return this.View();
        }

        public IActionResult Edit(int id)
        {
            if (!this.IsAdmin)
            {
                return this.RedirectToLogin();
            }

            var post = this._postService.GetById(id);

            if (post == null)
            {
                return this.NotFound();
            }

            this.ViewModel["title"] = post.Title;
            this.ViewModel["content"] = post.Content;

            return this.View();
        }

        [HttpPost]
        public IActionResult Edit(int id, PostModel model)
        {
            if (!this.IsAdmin)
            {
                return this.RedirectToLogin();
            }

            if (!this.IsValidModel(model))
            {
                this.ShowError(EDIT_ERROR);
                return this.View();
            }

            this._postService.Update(id, model.Title, model.Content);

            return this.Redirect("/admin/posts");
        }

        public IActionResult Delete(int id)
        {
            if (!this.IsAdmin)
            {
                return this.RedirectToLogin();
            }

            var post = this._postService.GetById(id);

            if (post == null)
            {
                return this.NotFound();
            }

            this.ViewModel["title"] = post.Title;
            this.ViewModel["content"] = post.Content;
            this.ViewModel["id"] = id.ToString();

            return this.View();
        }

        [HttpPost]
        public IActionResult Confirm(int id)
        {
            if (!this.IsAdmin)
            {
                return this.RedirectToLogin();
            }

            this._postService.Delete(id);
            return this.Redirect("/admin/posts");
        }
    }
}
