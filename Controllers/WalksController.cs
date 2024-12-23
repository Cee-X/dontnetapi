using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.Models.DTO;
using NZWalks.Data;
using NZWalks.Models.Domain;
using NZWalks.Repositories;
using NZWalks.CustomActionFilter;
using Microsoft.AspNetCore.Authorization;
namespace NZWalks.Controllers;
[Authorize]
[ApiController]
[Route("[controller]")]

public class WalksController : ControllerBase {
    private readonly NZWalksDbContext _dbContext;
    private readonly IWalksRepository _walksRepository;
    private readonly IMapper _mapper;
    public WalksController(NZWalksDbContext dbContext, IWalksRepository walksRepository, IMapper mapper)
    {
        _dbContext = dbContext;
        _walksRepository = walksRepository;
        _mapper = mapper;
    }

    [HttpGet(Name = "Walks")]

    public async Task<IActionResult> GetAll([FromQuery] string? filterOn = null, [FromQuery] string? filterQuery = null, 
    [FromQuery] string? sortBy = null, [FromQuery] bool isAscending = true,
    [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var walks = await _walksRepository.GetAll(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);
        var walksDto = _mapper.Map<List<WalkDto>>(walks);
        return Ok(walksDto);
    }

    [HttpGet("{id}")]
    public async  Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var walk = await _walksRepository.GetById(id);
        if (walk == null)
        {
            return NotFound();
        }
        var walkDto = _mapper.Map<WalkDto>(walk);
        return Ok(walkDto);
    }

    [HttpPost(Name = "Walks")]
    [ValidateModel]
    public async Task<IActionResult> Create([FromBody] RequestWalkDto requestWalkDto)
    {
        var walk = _mapper.Map<Walk>(requestWalkDto);
        walk = await _walksRepository.Create(walk);
        var walkDto = _mapper.Map<WalkDto>(walk);
        return CreatedAtAction(nameof(GetById), new {id = walk.Id}, walkDto);
    }

    [HttpPut("{id}")]
    [ValidateModel]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkDto updateWalkDto)
    {
       
        var walk = _mapper.Map<Walk>(updateWalkDto);
        var walkToUpdate = await _walksRepository.Update(id,walk);
        if (walkToUpdate == null)
        {
            return NotFound();
        }
        var walkDto = _mapper.Map<WalkDto>(walkToUpdate);
       
        return Ok(walkDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var walk = await _walksRepository.Delete(id);
        if (walk == null)
        {
            return NotFound();
        }
        var walkDto = _mapper.Map<WalkDto>(walk);
        return Ok(walkDto);
    }
}



