namespace MotorDepotApp.Models
{
    public class CargoType
    {
        public string Name { get; set; } = string.Empty;
        public int MinRequiredExperienceYears { get; set; }

        public CargoType(string name, int minRequiredExperienceYears)
        {
            Name = name;
            MinRequiredExperienceYears = minRequiredExperienceYears;
        }

        public override string ToString() => Name;
    }
}
