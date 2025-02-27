using NZWalks.Models.Domain;

namespace  NZWalks.Models.DTO
{
    public class WalkDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Descriptiion { get; set; }

        public double LengthInKm { get; set; }

        public string? WalkingImageUrl { get; set; }

         public DifficultyDto Difficulty { get; set; }

        public RegionDTO Region { get; set; }
    
    }
}