using System.ComponentModel.DataAnnotations;

namespace NZWalks.Models.DTO
{
    public class UpdateRegionDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Code must be at least 3 characters")]
        [MaxLength(3, ErrorMessage = "Code must be at most 3 characters")]
        public string Code { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Name must be at most 50 characters")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
