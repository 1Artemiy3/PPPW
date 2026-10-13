using MotorDepotApp.Models;
using MotorDepotApp.Services.Interfaces;

namespace MotorDepotApp.Services.Implementations
{
    public class StandardPaymentCalculator : IPaymentCalculator
    {
        public decimal CalculatePayment(Trip trip)
        {
            decimal distanceRate = (decimal)trip.Request.DistanceKm * 15m;
            decimal weightBonus = (decimal)trip.Request.CargoWeightKg * 0.05m;
            return distanceRate + weightBonus;
        }
    }
}
