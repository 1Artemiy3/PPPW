using System.Collections.Generic;
using MotorDepotApp.Models;

namespace MotorDepotApp.Services.Interfaces
{
    public interface IVehicleSelectionStrategy
    {
        IVehicle SelectVehicle(IEnumerable<IVehicle> vehicles, double cargoWeightKg);
    }
}
