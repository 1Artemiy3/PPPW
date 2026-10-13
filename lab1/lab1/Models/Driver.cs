using System;

namespace MotorDepotApp.Models
{
    public class Driver
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string FullName { get; set; } = string.Empty;
        public int ExperienceYears { get; set; }
        public DriverStatus Status { get; set; } = DriverStatus.Available;
        public decimal Balance { get; set; }

        public Driver(string fullName, int experienceYears)
        {
            FullName = fullName;
            ExperienceYears = experienceYears;
        }
    }
}
