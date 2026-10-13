using System;

namespace MotorDepotApp.Models
{
    public class Trip
    {
        public Guid Id { get; } = Guid.NewGuid();
        public TripRequest Request { get; }
        public Driver Driver { get; }
        public IVehicle Vehicle { get; }
        public TripStatus Status { get; set; } = TripStatus.Planned;
        public string Logs { get; set; } = string.Empty;

        public Trip(TripRequest request, Driver driver, IVehicle vehicle)
        {
            Request = request;
            Driver = driver;
            Vehicle = vehicle;
        }
    }
}
