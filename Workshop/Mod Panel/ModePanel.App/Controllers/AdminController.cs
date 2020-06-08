namespace ModePanel.App.Controllers
{
    using System;
    using System.Linq;
    using Infrastructure;
    using Services;
    using Services.Contracts;
    using SimpleMvc.Framework.Contracts;

    public class AdminController : BaseController
    {
        private readonly IUserService _userService;

        public AdminController()
        {
            this._userService = new UserService();
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
                                                {(u.IsApproved ? string.Empty : $@"<a class=""btn btn-primary"" href=""/admin/approve?id={u.Id}"">Approve</a>")}
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
    }
}
