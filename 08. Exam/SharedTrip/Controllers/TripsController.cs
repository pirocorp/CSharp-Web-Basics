namespace SharedTrip.Controllers
{
    using System;
    using System.Globalization;
    using Services;
    using SIS.HTTP;
    using SIS.MvcFramework;
    using ViewModels.Trips;

    public class TripsController : Controller
    {
        private const string DATE_TIME_FORMAT = "dd.MM.yyyy HH:mm";

        private readonly ITripsService _tripsService;
        private readonly IUsersTripsService _usersTripsService;

        public TripsController(ITripsService tripsService, IUsersTripsService usersTripsService)
        {
            this._tripsService = tripsService;
            this._usersTripsService = usersTripsService;
        }

        public HttpResponse All()
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var viewModel = new AllListModel()
            {
                Trips = this._tripsService.GetAll(),
            };

            return this.View(viewModel);
        }

        public HttpResponse Add()
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Add(AddTripInputModel input)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (string.IsNullOrWhiteSpace(input.StartPoint))
            {
                return this.Error("Start point can't be empty.");
            }

            if (string.IsNullOrWhiteSpace(input.EndPoint))
            {
                return this.Error("Start point can't be empty.");
            }

            if (!DateTime.TryParseExact(input.DepartureTime, DATE_TIME_FORMAT, 
                CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, 
                out var departureTime))
            {
                return this.Error("Invalid date time format.");
            }

            if (input.Seats < 2 || input.Seats > 6)
            {
                return this.Error("Invalid seats count.");
            }

            if (string.IsNullOrWhiteSpace(input.Description)
                || input.Description.Length > 80)
            {
                return this.Error("Description is required and can be max 80 characters long.");
            }

            this._tripsService.Create(input.StartPoint, input.EndPoint, departureTime, 
                input.ImagePath, input.Seats, input.Description);

            return this.Redirect("/Trips/All");
        }

        public HttpResponse Details(string tripId)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var viewModel = this._tripsService.Details(tripId);

            return this.View(viewModel);
        }

        public HttpResponse AddUserToTrip(string tripId)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (!this._tripsService.TripExists(tripId))
            {
                return this.Error($"Trip with id {tripId} doesn't exists trip.");
            }

            if (this._usersTripsService.UserExistsOnTrip(this.User, tripId))
            {
                return this.Error("User can register only once for trip.");
            }

            if (!this._tripsService.CheckForFreeSeats(tripId))
            {
                return this.Error("Sorry there is no free seats for this trip.");
            }

            this._usersTripsService.RegisterUserForTrip(this.User, tripId);
            return this.Redirect("/");
        }
    }
}
