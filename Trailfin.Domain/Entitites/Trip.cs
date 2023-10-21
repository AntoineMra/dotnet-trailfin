using System;

namespace Trailfin.Domain.Entities
{
    public class Trip
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Location { get; set; }
        public int DifficultyLevel { get; set; }
        public int MaxParticipants { get; set; }
        public decimal Price { get; set; }
        public int UserId { get; set; }
    }
}
