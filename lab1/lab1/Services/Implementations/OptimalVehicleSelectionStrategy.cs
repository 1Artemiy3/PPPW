using System.Collections.Generic;
using System.Linq;
using MotorDepotApp.Models;
using MotorDepotApp.Services.Interfaces;

namespace MotorDepotApp.Services.Implementations
{
    public class OptimalVehicleSelectionStrategy : IVehicleSelectionStrategy
    {
        public IVehicle SelectVehicle(IEnumerable<IVehicle> vehicles, double cargoWeightKg)
        {
            return vehicles
                .Where(v => v.Status == VehicleStatus.Available && v.LoadCapacityKg >= cargoWeightKg)
                .OrderBy(v => v.LoadCapacityKg)
                .FirstOrDefault();
        }
    }
}
