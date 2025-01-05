using NZWalks.Data;
using NZWalks.Models.Domain;

namespace NZWalks.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly NZWalksDbContext _dbContext;

        public ImageRepository(NZWalksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Image> Upload(Image image)
        {
            await _dbContext.Images.AddAsync(image);
            await _dbContext.SaveChangesAsync();
            return image;
        }
    }
}