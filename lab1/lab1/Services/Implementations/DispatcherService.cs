using System.Collections.Generic;
using MotorDepotApp.Models;
using MotorDepotApp.Services.Interfaces;

namespace MotorDepotApp.Services.Implementations
{
    public class DispatcherService : IDispatcherService
    {
        private readonly IVehicleSelectionStrategy _vehicleStrategy;
        private readonly IDriverSelectionStrategy _driverStrategy;

        public DispatcherService(IVehicleSelectionStrategy vehicleStrategy, IDriverSelectionStrategy driverStrategy)
        {
            _vehicleStrategy = vehicleStrategy;
            _driverStrategy = driverStrategy;
        }

        public Trip CreateTrip(TripRequest request, IEnumerable<Driver> drivers, IEnumerable<IVehicle> vehicles)
        {
            var vehicle = _vehicleStrategy.SelectVehicle(vehicles, request.CargoWeightKg);
            if (vehicle == null)
                return null;

            var driver = _driverStrategy.SelectDriver(drivers, request, vehicle);
            if (driver == null)
                return null;

            vehicle.Status = VehicleStatus.OnTrip;
            driver.Status = DriverStatus.OnTrip;
            request.IsAssigned = true;

            return new Trip(request, driver, vehicle)
            {
                Status = TripStatus.InProgress,
                Logs = "Рейс розпочато."
            };
        }
    }
}
