namespace SharedTrip.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Data.Models;
    using Models.Trips;

    public class TripService : ITripService
    {
        private readonly ApplicationDbContext dbContext;

        public TripService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public bool Exists(string tripId)
            => this.dbContext.Trips.Any(t => t.Id == tripId);

        public TripDetailsServiceModel Details(string userId, string tripId)
            => this.dbContext.Trips
                .Where(t => t.Id == tripId)
                .Select(t => new TripDetailsServiceModel()
                {
                    Id = t.Id,
                    StartPoint = t.StartPoint,
                    EndPoint = t.EndPoint,
                    DepartureTime = t.DepartureTime.ToString(DataConstants.DepartureDateTimeFormat),
                    Seats = t.Seats - t.UserTrips.Count(),
                    Description = t.Description,
                    ImagePath = t.ImagePath,
                    UserIsInTrip = t.UserTrips.Any(ut => ut.UserId == userId)
                })
                .FirstOrDefault();

        public IEnumerable<TripsListingModel> All()
            => this.dbContext.Trips
                .OrderByDescending(t => t.DepartureTime)
                .Select(t => new TripsListingModel()
                {
                    Id = t.Id,
                    StartPoint = t.StartPoint,
                    EndPoint = t.EndPoint,
                    DepartureTime = t.DepartureTime.ToString(DataConstants.DepartureDateTimeFormat),
                    Seats = t.Seats - t.UserTrips.Count()
                })
                .ToList();

        public void Create(
            string startPoint,
            string endPoint,
            DateTime departureTime,
            int seats,
            string description,
            string imagePath)
        {
            var trip = new Trip()
            {
                StartPoint = startPoint,
                EndPoint = endPoint,
                DepartureTime = departureTime,
                Seats = seats,
                Description = description,
                ImagePath = imagePath
            };

            this.dbContext.Add(trip);
            this.dbContext.SaveChanges();
        }
    }
}