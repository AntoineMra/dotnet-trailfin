using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Trailfin.Application.interfaces;
using Trailfin.Application.Models;
using Trailfin.Domain.Entities;

namespace Trailfin.Application.Services
{
    public class TripService : ITripService
    {
        private readonly List<Trip> _trips;

        public TripService()
        {
            _trips = new List<Trip>();
        }

        public Task<Result<Trip>> CreateTrip(Trip trip)
        {
            _trips.Add(trip);
            return Task.FromResult(Result.Success(trip));
        }

        public Task<Result<Trip>> GetTrip(int id)
        {
            var index = _trips.Find(trip => trip.Id == id);

            if (index == null)
            {
                return Task.FromResult(Result.Failure<Trip>($"Trip with id {id} not found"));
            }
            return Task.FromResult(Result.Success(index));
        }

        public Task<Result<List<Trip>>> GetAllTrips()
        {
            return Task.FromResult(Result.Success(_trips));
        }

        public Task<Result<Trip>> UpdateTrip(int id, Trip trip)
        {
            var index = _trips.FindIndex(t => t.Id == id);
            if (index == -1)
            {
                return Task.FromResult(Result.Failure<Trip>($"Trip with id {id} not found"));
            }

            _trips[index] = trip;
            return Task.FromResult(Result.Success(trip));
        }

        public Task<Result<bool>> DeleteTrip(int id)
        {
            var index = _trips.FindIndex(t => t.Id == id);
            if (index == -1)
            {
                return Task.FromResult(Result.Failure<bool>($"Trip with id {id} not found"));
            }

            _trips.RemoveAt(index);
            return Task.FromResult(Result.Success(true));
        }
    }
}
