using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using NZWalks.Models.Domain;
using NZWalks.Models.DTO;
using NZWalks.Data;
using NZWalks.Repositories;
using NZWalks.CustomActionFilter;
using Microsoft.AspNetCore.Authorization;
namespace NZWalks.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class RegionsController : ControllerBase
{
    private readonly NZWalksDbContext _context;
    private readonly IRegionsRepository _regionsRepository;
    private readonly IMapper _mapper;
    public RegionsController(NZWalksDbContext context, IRegionsRepository regionsRepository, IMapper mapper)
    {
        _context = context;
        _regionsRepository = regionsRepository;
        _mapper = mapper;

    }
    
   [HttpGet(Name = "Regions")]
   public async Task<IActionResult> GetALL()
   {
        //get all regions from the database
        var regions =  await _regionsRepository.GetAll();
        //convert the list of regions to a list of RegionDTO
        var regionsDTO = _mapper.Map<List<RegionDTO>>(regions);
        //return the list of RegionDTO
        return Ok(regionsDTO);
   }

   [HttpGet("{id}")]
   
   public async Task<IActionResult> GetById([FromRoute] Guid id)
   {
        //get the region with the specified Id
        var region = await _regionsRepository.GetById(id);
        if (region == null )
        {
            return NotFound();
        }
        //convert the region to a regionDTO
        var regionDTO = _mapper.Map<RegionDTO>(region);
        return Ok(regionDTO);
   }

   [HttpPost(Name = "Regions")]
   [ValidateModel]
   public async Task<IActionResult> Create([FromBody] RequestRegionDTO requestRegionDTO)
   {
        var region = _mapper.Map<Region>(requestRegionDTO);
        region = await _regionsRepository.Create(region);
        var regionDTO = _mapper.Map<RegionDTO>(region);
        return CreatedAtAction(nameof(GetById), new { id = region.Id }, regionDTO);
   }

   [HttpPut("{id}")]
   [ValidateModel]
   public async Task<IActionResult> Update([FromRoute] Guid id,[FromBody] UpdateRegionDto updateRegionDto)
   { 
        
        var region = _mapper.Map<Region>(updateRegionDto);
        region = await _regionsRepository.Update(id, region );
        if (region == null)
        {
            return NotFound();
        }
        var regionDTO = _mapper.Map<RegionDTO>(region);
        return Ok(regionDTO);
   }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        //get the region with the specified Id
        var region = await _regionsRepository.Delete(id);
        if (region == null)
        {
            return NotFound();
        }
       //return deleted regionDto
        var regionDTO = _mapper.Map<RegionDTO>(region);
        return Ok(regionDTO);
    }

}



