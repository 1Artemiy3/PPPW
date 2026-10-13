namespace MotorDepotApp.Models
{
    public enum VehicleStatus
    {
        Available,
        OnTrip,
        Broken,
        UnderRepair
    }

    public enum DriverStatus
    {
        Available,
        OnTrip
    }

    public enum TripStatus
    {
        Planned,
        InProgress,
        BrokenOnTrip,
        Completed
    }
}
