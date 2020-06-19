namespace SharedTrip.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using ViewModels.Trips;

    public class TripsService : ITripsService
    {
        private readonly ApplicationDbContext _db;

        public TripsService(ApplicationDbContext db)
        {
            this._db = db;
        }

        public void Create(string startPoint, string endPoint, 
            DateTime departureTime, string imagePath, int seats,
            string description)
        {
            var trip = new Trip()
            {
                StartPoint = startPoint,
                EndPoint = endPoint,
                DepartureTime = departureTime,
                ImagePath = imagePath,
                Seats = seats,
                Description = description
            };

            this._db.Trips.Add(trip);
            this._db.SaveChanges();
        }

        public IEnumerable<AllTripListModel> GetAll()
            => this._db.Trips
                .Select(t => new AllTripListModel()
                {
                    Id = t.Id,
                    StartPoint = t.StartPoint,
                    EndPoint = t.EndPoint,
                    Seats = t.Seats,
                    DepartureTime = t.DepartureTime,
                })
                .ToList();

        public DetailsTripModel Details(string id)
            => this._db.Trips
                .Where(t => t.Id == id)
                .Select(t => new DetailsTripModel()
                {
                    Id = t.Id,
                    StartPoint = t.StartPoint,
                    EndPoint = t.EndPoint,
                    DepartureTime = t.DepartureTime,
                    Seats = t.Seats,
                    Description = t.Description,
                    ImagePath = t.ImagePath,
                })
                .FirstOrDefault();

        public void ReduceSeats(string tripId)
        {
            var trip = this._db.Trips.Find(tripId);

            trip.Seats -= 1;

            this._db.Entry(trip).State = EntityState.Modified;
            this._db.SaveChanges();
        }

        public bool CheckForFreeSeats(string tripId)
            => this._db.Trips
                .Any(t => t.Id == tripId
                       && t.Seats > 0);

        public bool TripExists(string tripId)
            => this._db.Trips.Any(t => t.Id == tripId);
    }
}
