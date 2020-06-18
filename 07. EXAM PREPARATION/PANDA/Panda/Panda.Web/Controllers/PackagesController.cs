namespace Panda.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using Models;
    using Services;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Attributes.Http;
    using SIS.MvcFramework.Attributes.Security;
    using SIS.MvcFramework.Result;
    using ViewModels.Packages;

    public class PackagesController : Controller
    {
        private readonly IPackagesService packagesService;
        private readonly IUsersService usersService;

        public PackagesController(IPackagesService packagesService, 
            IUsersService usersService)
        {
            this.packagesService = packagesService;
            this.usersService = usersService;
        }

        [Authorize]
        public IActionResult Create()
        {
            var list = this.usersService.GetUsernames();

            return this.View(list);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(CreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect("/Packages/Create");
            }

            this.packagesService.Create(input.Description, input.Weight,
                input.ShippingAddress, input.RecipientName);

            return this.Redirect("Packages/Pending");
        }

        [Authorize]
        public IActionResult Delivered()
        {
            var viewModel = new PackagesListViewModel()
            {
                Packages = this.GetPackages(PackageStatus.Delivered),
            };

            return this.View(viewModel);
        }

        [Authorize]
        public IActionResult Pending()
        {
            var viewModel = new PackagesListViewModel()
            {
                Packages = this.GetPackages(PackageStatus.Pending),
            };

            return this.View(viewModel);
        }

        [Authorize]
        public IActionResult Deliver(string id)
        {
            this.packagesService.Deliver(id);
            return this.Redirect("/Packages/Delivered");
        }

        private IEnumerable<PackageViewModel> GetPackages(PackageStatus status)
            => this.packagesService
                .GetAllByStatus(status)
                .Select(p => new PackageViewModel()
                {
                    Id = p.Id,
                    Description = p.Description,
                    ShippingAddress = p.ShippingAddress,
                    Weight = p.Weight,
                    RecipientName = p.Recipient.Username
                })
                .ToList();
    }
}
