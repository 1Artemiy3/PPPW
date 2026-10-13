using System.Collections.Generic;
using MotorDepotApp.Models;

namespace MotorDepotApp.Services.Interfaces
{
    public interface IDispatcherService
    {
        Trip CreateTrip(TripRequest request, IEnumerable<Driver> drivers, IEnumerable<IVehicle> vehicles);
    }
}
