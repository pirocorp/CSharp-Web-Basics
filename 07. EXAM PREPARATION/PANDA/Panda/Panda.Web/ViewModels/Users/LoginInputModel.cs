namespace Panda.Web.ViewModels.Users
{
    using SIS.MvcFramework.Attributes.Validation;

    public class LoginInputModel
    {
        [RequiredSis]
        [StringLengthSis(5, 20, "Invalid credentials")]
        public string Username { get; set; }

        [RequiredSis]
        public string Password { get; set; }
    }
}
