using System;
using System.Collections.Generic;
using MotorDepotApp.Models;
using MotorDepotApp.Services.Interfaces;

namespace MotorDepotApp.Services.Implementations
{
    public class RandomRequestGenerator : IRequestGenerator
    {
        private readonly Random _random = new Random();
        private readonly List<CargoType> _cargoTypes = new List<CargoType>()
        {
            new CargoType("Звичайні товари", minRequiredExperienceYears: 1),
            new CargoType("Будівельні матеріали", minRequiredExperienceYears: 2),
            new CargoType("Швидкопсувні продукти", minRequiredExperienceYears: 3),
            new CargoType("Небезпечний вантаж (ADR)", minRequiredExperienceYears: 5)
        };

        private readonly string[] _destinations = { "Київ", "Львів", "Одеса", "Дніпро", "Харків", "Полтава", "Вінниця" };

        public IEnumerable<TripRequest> GenerateDailyRequests(int count)
        {
            var list = new List<TripRequest>();
            for (int i = 0; i < count; i++)
            {
                string dest = _destinations[_random.Next(_destinations.Length)];
                var cargo = _cargoTypes[_random.Next(_cargoTypes.Count)];
                double weight = Math.Round(_random.NextDouble() * (12000 - 500) + 500, 0);
                double distance = Math.Round(_random.NextDouble() * (800 - 50) + 50, 0);

                list.Add(new TripRequest(dest, weight, cargo, distance));
            }
            return list;
        }
    }
}
