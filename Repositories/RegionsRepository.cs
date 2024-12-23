using Microsoft.EntityFrameworkCore;
using NZWalks.Models.Domain;
using NZWalks.Models.DTO;
using NZWalks.Data;

namespace NZWalks.Repositories
{
    public class RegionsRepository : IRegionsRepository
    {
        private readonly NZWalksDbContext _dbContext;
        public RegionsRepository(NZWalksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Region>> GetAll()
        {
            return await _dbContext.Regions.ToListAsync();
        }

        public async Task<Region> GetById(Guid id)
        {
            var region= await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (region == null)
            {
                return null;
            }
            
            return region;
        }

        public async Task<Region> Create(Region region)
        {
            await _dbContext.Regions.AddAsync(region);
            await _dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region> Update(Guid id, Region region)
        {
            var regionToUpdate = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (regionToUpdate == null)
            {
                return null;
            }
            
            regionToUpdate.Code = region.Code;
            regionToUpdate.Name = region.Name;
            regionToUpdate.RegionImageUrl = region.RegionImageUrl;

            await _dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region> Delete(Guid id)
        {
            var region = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (region == null)
            {
                return null;
            }

            _dbContext.Regions.Remove(region);
            await _dbContext.SaveChangesAsync();
            
            return region;
        }

    }
    
}