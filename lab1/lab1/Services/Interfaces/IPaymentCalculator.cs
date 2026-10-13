using MotorDepotApp.Models;

namespace MotorDepotApp.Services.Interfaces
{
    public interface IPaymentCalculator
    {
        decimal CalculatePayment(Trip trip);
    }
}
