using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NZWalks.Models.Domain;
using NZWalks.Models.DTO;
using NZWalks.Repositories;

namespace NZWalks.Controllers;


[ApiController]
[Route("[controller]")]
[Authorize]

public class ImagesController : ControllerBase
{
    private readonly IImageRepository _imageRepository;
    private readonly IMapper _mapper;
    public ImagesController(IImageRepository imageRepository, IMapper mapper)
    {
        _imageRepository = imageRepository;
        _mapper = mapper;
    }
    [HttpPost]
    public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto imageUploadRequestDto)
    {
        ValidateFileUpload(imageUploadRequestDto);
        if (ModelState.IsValid)
        {
            var imageDomainModel = new Image 
            {
                File = imageUploadRequestDto.File,
                FileName = imageUploadRequestDto.FileName,
                FileDescription = imageUploadRequestDto.FileDescription,
                FileExtension = Path.GetExtension(imageUploadRequestDto.File.FileName),
                FileSizeInBytes = imageUploadRequestDto.File.Length,
 
            };

            await _imageRepository.Upload(imageDomainModel);
            return Ok(imageDomainModel);
        }
        return BadRequest(ModelState);
        
    }
    private void ValidateFileUpload(ImageUploadRequestDto request)
    {
        if(request == null) 
        {
            throw new ValidationException("Request is null");
        }
        var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
        if (!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)) )
        {
            ModelState.AddModelError("File", "Invalid file extension");
        }
        if (request.File.Length > 10485760)
        {
            ModelState.AddModelError("File", "File size is too large");
        }
    }
    
}
