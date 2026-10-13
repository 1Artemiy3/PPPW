using System.Collections.Generic;
using System.Linq;
using MotorDepotApp.Models;
using MotorDepotApp.Services.Interfaces;

namespace MotorDepotApp.Services.Implementations
{
    public class ExperienceDriverSelectionStrategy : IDriverSelectionStrategy
    {
        public Driver SelectDriver(IEnumerable<Driver> drivers, TripRequest request, IVehicle vehicle)
        {
            int distanceExperienceBonus = (int)(request.DistanceKm / 300);
            int totalRequiredExperience = request.CargoType.MinRequiredExperienceYears 
                                          + vehicle.DrivingComplexity 
                                          + distanceExperienceBonus;

            return drivers
                .Where(d => d.Status == DriverStatus.Available && d.ExperienceYears >= totalRequiredExperience)
                .OrderBy(d => d.ExperienceYears)
                .FirstOrDefault();
        }
    }
}
