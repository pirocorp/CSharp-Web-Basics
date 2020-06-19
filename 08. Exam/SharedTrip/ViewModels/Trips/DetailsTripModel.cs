namespace SharedTrip.ViewModels.Trips
{
    using System;

    public class DetailsTripModel
    {
        private const string DATE_TIME_FORMAT = "dd.MM.yyyy HH:mm";

        public string Id { get; set; }

        public string StartPoint { get; set; }

        public string EndPoint { get; set; }

        public DateTime DepartureTime { get; set; }

        public int Seats { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public string FormattedDepartureTime => this.DepartureTime.ToString(DATE_TIME_FORMAT);
    }
}
