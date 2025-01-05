using AutoMapper;
using NZWalks.Models.Domain;
using NZWalks.Models.DTO;

namespace  NZWalks.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Region, RegionDTO>().ReverseMap();
            CreateMap<Region, RequestRegionDTO>().ReverseMap();
            CreateMap<Region, UpdateRegionDto>().ReverseMap();
            CreateMap<Walk, WalkDto>().ReverseMap();
            CreateMap<Walk, RequestWalkDto>().ReverseMap();
            CreateMap<Walk, UpdateWalkDto>().ReverseMap();
            CreateMap<Difficulty, DifficultyDto>().ReverseMap();
            CreateMap<Image, ImageUploadRequestDto>().ReverseMap();
        }
    }
}
