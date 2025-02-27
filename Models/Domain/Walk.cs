namespace NZWalks.Models.Domain
{
    public class Walk
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string  Descriptiion { get; set; }
        public double LengthInKm { get; set; }
        public string?  WalkingImageUrl { get; set; }

        public Guid DifficultyId { get; set; }  // Changed from DifficultId to DifficultyId

        public Difficulty Difficulty { get; set; }

        public Guid RegionId { get; set; }

        public Region Region { get; set; }
    }
}