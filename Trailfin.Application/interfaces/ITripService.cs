using CSharpFunctionalExtensions;
using Trailfin.Application.Models;
using Trailfin.Domain.Entities;

namespace Trailfin.Application.interfaces
{
    public interface ITripService
    {
        Task<Result<Trip>> CreateTrip(Trip trip);

        Task<Result<Trip>> UpdateTrip(int id, Trip trip);

        Task<Result<bool>> DeleteTrip(int tripId);

        Task<Result<List<Trip>>> GetAllTrips();

        Task<Result<Trip>> GetTrip(int tripId);
    }
}
