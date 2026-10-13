using MotorDepotApp.Models;

namespace MotorDepotApp.Services.Interfaces
{
    public interface ITripSimulator
    {
        void ProgressTrip(Trip trip);
        void RequestRepair(Trip trip);
        void RepairVehicle(Trip trip);
        void CompleteTrip(Trip trip);
    }
}
