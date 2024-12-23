using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.Data;
using NZWalks.Models.Domain;

namespace NZWalks.Repositories
{
    public class WalksRepository : IWalksRepository
    {
        private readonly NZWalksDbContext _dbContext;
        public WalksRepository(NZWalksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Walk>> GetAll(string? filterOn = null, string? filterQuery = null,string? sortBy = null, bool isAscending = true,
        int pageNumber = 1, int pageSize = 10)
        {
            var walks = _dbContext.Walks.Include(x => x.Difficulty).Include(x => x.Region).AsQueryable();
            if (string.IsNullOrWhiteSpace(filterOn) == false  && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
            }
            if (string.IsNullOrWhiteSpace(sortBy) == false) 
            {
                if (sortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }
                else if(sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                 {
                    walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                }
            }
            var skipResult = (pageNumber - 1 ) * pageSize;

            return await walks.Skip(skipResult).Take(pageSize).ToListAsync();
        }
        public async Task<Walk> GetById(Guid id)
        {
            var walk = await _dbContext.Walks.Include(x => x.Difficulty).Include(x => x.Region).FirstOrDefaultAsync(x => x.Id == id);
            if (walk == null)
            {
                return null;
            }
            return walk;
        }
        
        public async Task<Walk> Create(Walk walk)
        {
            await _dbContext.Walks.AddAsync(walk);
            await _dbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk> Update(Guid id, Walk walk)
        {
            var walkToUpdate = await _dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (walkToUpdate == null)
            {
                return null;
            }
            walkToUpdate.Name = walk.Name;
            walkToUpdate.Descriptiion = walk.Descriptiion;
            walkToUpdate.LengthInKm = walk.LengthInKm;
            walkToUpdate.WalkingImageUrl = walk.WalkingImageUrl;
            walkToUpdate.DifficultyId = walk.DifficultyId;
            walkToUpdate.RegionId = walk.RegionId;
            await _dbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk> Delete(Guid id)
        {
            var walk = await _dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (walk == null){
                return null;
            }
            _dbContext.Walks.Remove(walk);
            await _dbContext.SaveChangesAsync();
            return walk;
        }
    }
}