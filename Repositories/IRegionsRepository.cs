
using NZWalks.Models.Domain;

namespace NZWalks.Repositories
{
    public interface IRegionsRepository
    {
         Task<IEnumerable<Region>> GetAll();
         Task<Region> GetById(Guid id);

         Task<Region> Create(Region region);

         Task<Region> Update(Guid id, Region region);

         Task<Region> Delete(Guid id);


    }
}

