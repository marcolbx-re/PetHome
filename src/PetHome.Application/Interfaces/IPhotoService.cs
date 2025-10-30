using Microsoft.AspNetCore.Http;
using PetHome.Application.Photos;

namespace PetHome.Application.Interfaces;

public interface IPhotoService
{

	Task<PhotoUploadResult> AddPhoto(IFormFile file);

	Task<string> DeletePhoto(string publicId);
}
