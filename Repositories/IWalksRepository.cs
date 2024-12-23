using NZWalks.Models.Domain;

namespace NZWalks.Repositories
{
    public interface IWalksRepository
    {
        Task<IEnumerable<Walk>> GetAll(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 10);
        Task<Walk> GetById(Guid id);
        Task<Walk> Create(Walk walk);
        Task<Walk> Update (Guid id, Walk walk);
        Task<Walk> Delete(Guid id);
    }

}