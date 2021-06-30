namespace SharedTrip.Services
{
    using System;
    using System.Collections.Generic;
    using Models.Trips;

    public interface ITripService
    {
        bool Exists(string tripId);

        TripDetailsServiceModel Details(string userId, string tripId);

        IEnumerable<TripsListingModel> All();

        void Create(string startPoint, string endPoint, DateTime departureTime,
            int seats, string description, string imagePath);
    }
}
