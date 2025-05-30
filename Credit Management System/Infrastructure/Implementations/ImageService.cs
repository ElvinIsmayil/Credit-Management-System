using Credit_Management_System.Infrastructure.Interfaces;

namespace Credit_Management_System.Infrastructure.Implementations
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string[] _allowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB

        public ImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<(string imageUrl, List<string> validationErrors)> SaveImageAsync(IFormFile imageFile, string folderName, string oldImageUrl = null)
        {
            var validationErrors = ValidateFileType(imageFile);
            if (validationErrors.Any())
            {
                return (null, validationErrors);
            }

            if (imageFile.Length > MaxFileSize)
            {
                validationErrors.Add($"File size exceeds the limit of {MaxFileSize / (1024 * 1024)} MB.");
                return (null, validationErrors);
            }

            // Delete old image if provided and exists
            if (!string.IsNullOrEmpty(oldImageUrl))
            {
                DeleteImage(oldImageUrl);
            }

            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string uploadPath = Path.Combine(wwwRootPath, "uploads", folderName);

            // Ensure the directory exists
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Generate a unique file name
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            string filePath = Path.Combine(uploadPath, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            // Return the relative URL
            string imageUrl = $"/uploads/{folderName}/{fileName}";
            return (imageUrl, null); // No errors, return null for errors list
        }

        public void DeleteImage(string imageUrl)
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
                    // Log the exception (e.g., file in use)
                    // You might want to use ILogger here
                    Console.WriteLine($"Error deleting image: {ex.Message}");
                }
            }
        }

        public List<string> ValidateFileType(IFormFile imageFile)
        {
            var errors = new List<string>();

            if (imageFile == null)
            {
                errors.Add("Image file is required.");
                return errors;
            }

            var fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            if (!_allowedImageExtensions.Contains(fileExtension))
            {
                errors.Add($"Invalid file type. Only {string.Join(", ", _allowedImageExtensions)} are allowed.");
            }

            return errors;
        }
    }
}

