using System;

namespace MotorDepotApp.Models
{
    public interface IVehicle
    {
        Guid Id { get; }
        string Model { get; }
        string LicensePlate { get; }
        double LoadCapacityKg { get; }
        int DrivingComplexity { get; }
        VehicleStatus Status { get; set; }
    }

    public abstract class VehicleBase : IVehicle
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Model { get; set; } = string.Empty;
        public string LicensePlate { get; set; } = string.Empty;
        public double LoadCapacityKg { get; set; }
        public int DrivingComplexity { get; set; }
        public VehicleStatus Status { get; set; } = VehicleStatus.Available;

        protected VehicleBase(string model, string licensePlate, double capacity, int complexity)
        {
            Model = model;
            LicensePlate = licensePlate;
            LoadCapacityKg = capacity;
            DrivingComplexity = complexity;
        }
    }

    public class StandardTruck : VehicleBase
    {
        public StandardTruck(string model, string licensePlate, double capacity, int complexity)
            : base(model, licensePlate, capacity, complexity) { }
    }

    public class HeavyTruck : VehicleBase
    {
        public HeavyTruck(string model, string licensePlate, double capacity, int complexity)
            : base(model, licensePlate, capacity, complexity) { }
    }
}
