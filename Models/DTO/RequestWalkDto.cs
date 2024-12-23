using System.ComponentModel.DataAnnotations;

namespace NZWalks.Models.DTO
{
    public class RequestWalkDto
    {
        [Required]
        [StringLength(50, ErrorMessage = "Name must be at most 50 characters")]
        public string  Name { get; set; }
        
        [Required]
        [StringLength(500, ErrorMessage = "Description must be at most 500 characters")]
        public string Descriptiion { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Length must be between 0 and 100 km")]
        public double LengthInKm { get; set; }
        public string? WalkingImageUrl { get; set; }

        [Required]
        public Guid DifficultyId { get; set; }

        [Required]
        public Guid RegionId { get; set; }
    }
}