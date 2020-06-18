namespace Panda.Web.Controllers
{
    using SIS.MvcFramework;
    using SIS.MvcFramework.Attributes.Http;
    using SIS.MvcFramework.Result;

    public class HomeController : Controller
    {
        /// <summary>
        /// Action for route /Home/Index
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return this.View();
        }

        /// <summary>
        /// Action for route /
        /// </summary>
        /// <returns></returns>
        [HttpGet(Url = "/")]
        public IActionResult IndexSlash()
        {
            return this.Index();
        }
    }
}
