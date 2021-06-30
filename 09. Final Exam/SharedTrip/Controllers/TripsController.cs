namespace SharedTrip.Controllers
{
    using System;
    using System.Globalization;
    using System.Linq;

    using MyWebServer.Controllers;
    using MyWebServer.Http;

    using SharedTrip.Models.Trips;
    using SharedTrip.Services;

    using static Data.DataConstants;

    public class TripsController : Controller
    {
        private readonly IValidator validator;
        private readonly ITripService tripService;
        private readonly IUserTripService userTripService;

        public TripsController(
            IValidator validator,
            ITripService tripService,
            IUserTripService userTripService)
        {
            this.validator = validator;
            this.tripService = tripService;
            this.userTripService = userTripService;
        }

        [Authorize]
        public HttpResponse Add() => this.View();

        [Authorize]
        [HttpPost]
        public HttpResponse Add(TripCreateFormModel model)
        {
            var modelErrors = this.validator.ValidateCreateTrip(model);

            var departureTime = DateTime(model);

            if (departureTime == null)
            {
                modelErrors.Add($"Date must be in format {DepartureDateTimeFormat}");
            }

            if (modelErrors.Any())
            {
                return this.Error(modelErrors);
            }

            this.tripService.Create(model.StartPoint,
                model.EndPoint,
                departureTime.Value,
                model.Seats,
                model.Description,
                model.ImagePath);

            return this.Redirect("/Trips/All");
        }

        [Authorize]
        public HttpResponse All()
        {
            var trips = this.tripService.All();

            return this.View(trips);
        }

        [Authorize]
        public HttpResponse Details(string tripId)
        {
            if (!this.tripService.Exists(tripId))
            {
                return this.NotFound();
            }

            var trip = this.tripService.Details(this.User.Id, tripId);

            return this.View(trip);
        }

        [Authorize]
        public HttpResponse AddUserToTrip(string tripId)
        {
            var userId = this.User.Id;

            if (this.userTripService.UserIsRegisteredForTrip(userId, tripId))
            {
                return this.Redirect($"/Trips/AddUserToTrip?tripId={tripId}");
            }

            this.userTripService.AddUserToTrip(userId, tripId);

            return this.Redirect("/");
        }

        private static DateTime? DateTime(TripCreateFormModel model)
        {
            DateTime? result = null;

            try
            {
                result = System.DateTime.ParseExact(model.DepartureTime, DepartureDateTimeFormat, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
            }

            return result;
        }
    }
}
