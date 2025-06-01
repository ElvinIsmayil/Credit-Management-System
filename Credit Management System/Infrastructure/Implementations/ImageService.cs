using Credit_Management_System.Infrastructure.Interfaces;
using System.Runtime.Intrinsics.Arm;
using static System.Net.Mime.MediaTypeNames;

namespace Credit_Management_System.Infrastructure.Implementations
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string[] _allowedImageExtensions = { ".jpg", ".jpeg", ".png"};
        private const long MaxFileSize = 5 * 1024 * 1024; 

        public ImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<(string? imageUrl, List<string> validationErrors)> SaveImageAsync(IFormFile? imageFile, string folderName, string? oldImageUrl = null)
        {
            var validationErrors = ValidateFileType(imageFile);
            if (validationErrors.Any())
            {
                return (null, validationErrors);
            }


            if (!string.IsNullOrEmpty(oldImageUrl))
            {
                DeleteImage(oldImageUrl);
            }

            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string uploadPath = Path.Combine(wwwRootPath, "uploads", folderName);

            try
            {
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
            }
            catch (Exception)
            {
                validationErrors.Add("Server error: Could not create upload directory.");
                return (null, validationErrors);
            }
            
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile?.FileName);
            string filePath = Path.Combine(uploadPath, fileName);

            try
            {

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile!.CopyToAsync(fileStream);
            }
            }catch (Exception)
            {
                validationErrors.Add("Server error: Could not save image file.");
                return (null, validationErrors);
            }

            string newImageUrl = $"/uploads/{folderName}/{fileName}";
            return (newImageUrl, new List<string>()); 
        }

        public void DeleteImage(string? imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string imagePath = Path.Combine(wwwRootPath, imageUrl.TrimStart('/'));

            if (File.Exists(imagePath))
            {
                try
                {
                    File.Delete(imagePath);
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Error deleting image: {ex.Message}");
                }
            }
        }

        public List<string> ValidateFileType(IFormFile? imageFile)
        {
            var errors = new List<string>();

            if (imageFile == null)
            {
                errors.Add("Image file is required.");
                return errors;
            }

            if (imageFile.Length == 0)
            {
                errors.Add("The uploaded file is empty.");
                return errors;
            }

            if (imageFile.Length > MaxFileSize)
            {
                errors.Add($"File size exceeds the limit of {MaxFileSize / (1024 * 1024)} MB.");
            }

            var fileExtension = Path.GetExtension(imageFile.FileName)?.ToLowerInvariant();
            if (string.IsNullOrEmpty(fileExtension) || !_allowedImageExtensions.Contains(fileExtension))
            {
                errors.Add($"Invalid file type. Only {string.Join(", ", _allowedImageExtensions)} are allowed.");
            }

            return errors;
        }
    }
}

