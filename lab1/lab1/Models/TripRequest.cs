using System;

namespace MotorDepotApp.Models
{
    public class TripRequest
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Destination { get; set; } = string.Empty;
        public double CargoWeightKg { get; set; }
        public CargoType CargoType { get; set; }
        public double DistanceKm { get; set; }
        public bool IsAssigned { get; set; }

        public TripRequest(string destination, double cargoWeightKg, CargoType cargoType, double distanceKm)
        {
            Destination = destination;
            CargoWeightKg = cargoWeightKg;
            CargoType = cargoType;
            DistanceKm = distanceKm;
        }
    }
}
