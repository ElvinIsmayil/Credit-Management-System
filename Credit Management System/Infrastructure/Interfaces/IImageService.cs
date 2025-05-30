namespace Credit_Management_System.Infrastructure.Interfaces
{
    public interface IImageService
    {
        Task<(string imageUrl, List<string> validationErrors)> SaveImageAsync(IFormFile imageFile, string folderName, string oldImageUrl = null);
        void DeleteImage(string imageUrl);
        List<string> ValidateFileType(IFormFile imageFile);
    }
}
