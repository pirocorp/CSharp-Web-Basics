namespace SharedTrip.Services
{
    using System;
    using System.Collections.Generic;
    using ViewModels.Trips;

    public interface ITripsService
    {
        void Create(string startPoint, string endPoint, DateTime departureTime, 
            string imagePath, int seats, string description);

        IEnumerable<AllTripListModel> GetAll();

        DetailsTripModel Details(string id);

        void ReduceSeats(string tripId);

        bool CheckForFreeSeats(string tripId);

        bool TripExists(string tripId);
    }
}
