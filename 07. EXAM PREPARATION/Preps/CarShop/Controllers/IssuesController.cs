namespace CarShop.Controllers
{
    using System.Linq;

    using Data;
    using Models.Issues;
    using MyWebServer.Controllers;
    using MyWebServer.Http;
    using Services;

    public class IssuesController : Controller
    {
        private readonly IUserService userService;
        private readonly CarShopDbContext dbContext;

        public IssuesController(IUserService userService,
            CarShopDbContext dbContext)
        {
            this.userService = userService;
            this.dbContext = dbContext;
        }

        [Authorize]
        public HttpResponse CarIssues(string carId)
        {
            if (!this.userService.UserIsMechanic(this.User.Id))
            {
                var userOwnsCar = this.dbContext.Cars
                    .Any(c => c.Id == carId && c.OwnerId == this.User.Id);

                if (!userOwnsCar)
                {
                    return this.Error("Car with this id doesn't exists.");
                }
            }

            var carIssues = this.dbContext.Cars
                .Where(c => c.Id == carId)
                .Select(c => new CarIssuesViewModel()
                {
                    CarId   = c.Id,
                    Model = c.Model,
                    Year = c.Year,
                    Issues = c.Issues
                        .Select(i => new IssueListingModel()
                        {
                            Id = i.Id,
                            Description = i.Description,
                            IsFixed = i.IsFixed
                        })
                })
                .FirstOrDefault();

            if (carIssues is null)
            {
                return this.Error($"Car with ID '{carId}' does not exists.");
            }

            return this.View(carIssues);
        }
    }
}
