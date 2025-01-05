using NZWalks.Data;
using NZWalks.Models.Domain;

namespace NZWalks.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly NZWalksDbContext _dbContext;
        public LocalImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, NZWalksDbContext dbContext)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }
        public async Task<Image> Upload(Image image)
        {
            var LocalFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", $"{image.FileName}{image.FileExtension}");
            //upload image to local folder 
            using var fileStream =  new FileStream(LocalFolderPath, FileMode.Create);
            await image.File.CopyToAsync(fileStream);

            //https://localhost:5001/Images/imagename.jpg

            var urlFilePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";
            image.FilePath = urlFilePath;

            //save the image to the database
            await _dbContext.Images.AddAsync(image);
            await _dbContext.SaveChangesAsync();

            return image;


        }
    }
}