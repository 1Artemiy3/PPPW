using System.Collections.Generic;
using MotorDepotApp.Models;

namespace MotorDepotApp.Services.Interfaces
{
    public interface IDriverSelectionStrategy
    {
        Driver SelectDriver(IEnumerable<Driver> drivers, TripRequest request, IVehicle vehicle);
    }
}
