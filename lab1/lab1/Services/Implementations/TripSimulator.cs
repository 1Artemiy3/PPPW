using System;
using MotorDepotApp.Models;
using MotorDepotApp.Services.Interfaces;

namespace MotorDepotApp.Services.Implementations
{
    public class TripSimulator : ITripSimulator
    {
        private readonly Random _random = new Random();
        private readonly IPaymentCalculator _paymentCalculator;

        public TripSimulator(IPaymentCalculator paymentCalculator)
        {
            _paymentCalculator = paymentCalculator;
        }

        public void ProgressTrip(Trip trip)
        {
            if (trip == null) return;

            if (trip.Status == TripStatus.Planned)
            {
                trip.Status = TripStatus.InProgress;
                trip.Logs = "Рейс розпочато.";
                return;
            }

            if (trip.Status != TripStatus.InProgress) return;

            // 25% ймовірність поломки під час руху
            if (_random.NextDouble() < 0.25)
            {
                trip.Status = TripStatus.BrokenOnTrip;
                trip.Vehicle.Status = VehicleStatus.Broken;
                trip.Logs = "Автомобіль зламався під час руху! Потрібна заявка на ремонт.";
            }
            else
            {
                // 75% ймовірність успішного завершення
                CompleteTrip(trip);
            }
        }

        public void RequestRepair(Trip trip)
        {
            if (trip == null || trip.Status != TripStatus.BrokenOnTrip) return;

            trip.Vehicle.Status = VehicleStatus.UnderRepair;
            trip.Logs += " -> Водій подав заявку на ремонт.";
        }

        public void RepairVehicle(Trip trip)
        {
            if (trip == null || trip.Vehicle.Status != VehicleStatus.UnderRepair) return;

            trip.Vehicle.Status = VehicleStatus.OnTrip;
            trip.Status = TripStatus.InProgress;
            trip.Logs += " -> Ремонт завершено, рейс продовжено.";
        }

        public void CompleteTrip(Trip trip)
        {
            trip.Status = TripStatus.Completed;
            decimal payment = _paymentCalculator.CalculatePayment(trip);

            trip.Driver.Balance += payment;

            trip.Driver.Status = DriverStatus.Available;
            trip.Vehicle.Status = VehicleStatus.Available;

            trip.Logs = $"Рейс успішно виконано. Нараховано виплату: {payment:C0}. Автомобіль справний та вільний.";
        }
    }
}
