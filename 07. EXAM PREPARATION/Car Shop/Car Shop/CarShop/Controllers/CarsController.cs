namespace CarShop.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Data.Models;
    using Models.Cars;
    using MyWebServer.Controllers;
    using MyWebServer.Http;
    using Services;

    public class CarsController : Controller
    {
        private readonly IUserService userService;
        private readonly IValidator validator;
        private readonly CarShopDbContext dbContext;

        public CarsController(
            IUserService userService,
            IValidator validator,
            CarShopDbContext dbContext)
        {
            this.userService = userService;
            this.validator = validator;
            this.dbContext = dbContext;
        }

        [Authorize]
        public HttpResponse Add()
        {
            if (this.userService.UserIsMechanic(this.User.Id))
            {
                return this.Unauthorized();
            }
            
            return this.View();
        }

        [HttpPost]
        [Authorize]
        public HttpResponse Add(AddCarFormModel model)
        {
            if (this.userService.UserIsMechanic(this.User.Id))
            {
                return this.Unauthorized();
            }

            var modelErrors = this.validator.ValidateCarCreation(model);

            if (modelErrors.Any())
            {
                return this.Error(modelErrors);
            }

            var car = new Car()
            {
                Model = model.Model,
                Year = model.Year,
                PictureUrl = model.Image,
                PlateNumber = model.PlateNumber,
                OwnerId = this.User.Id,
            };

            this.dbContext.Add(car);
            this.dbContext.SaveChanges();

            return this.Redirect("/Cars/All");
        }

        [Authorize]
        public HttpResponse All()
        {
            IQueryable<Car> query;

            if (this.userService.UserIsMechanic(this.User.Id))
            {
                query = this.dbContext.Cars
                    .Where(c => c.Issues.Any(i => !i.IsFixed));
            }
            else
            {
                query = this.dbContext.Cars
                    .Where(c => c.OwnerId == this.User.Id);
            }

            var cars = query
                .Select(c => new CarListingViewModel()
                {
                    Id = c.Id,
                    Model = c.Model,
                    Year = c.Year,
                    Image = c.PictureUrl,
                    PlateNumber = c.PlateNumber,
                    FixedIssues = c.Issues.Count(i => i.IsFixed),
                    RemainingIssues = c.Issues.Count(i => !i.IsFixed)
                })
                .ToList();

            return this.View(cars);
        }
    }
}
