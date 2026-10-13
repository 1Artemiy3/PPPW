using System.Collections.Generic;
using MotorDepotApp.Models;

namespace MotorDepotApp.Services.Interfaces
{
    public interface IRequestGenerator
    {
        IEnumerable<TripRequest> GenerateDailyRequests(int count);
    }
}
