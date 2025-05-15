namespace Credit_Management_System.Extensions
{
    public static class ImageExtension
    {
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png" };
        private const int MaxFileSizeInMB = 5;

        public static IEnumerable<string> ValidateFileType(this IFormFile file)
        {
            if (!IsImageContentType(file.ContentType))
                yield return "You can only upload image files!";

            if (!IsValidExtension(file.FileName))
                yield return "Only .jpg, .jpeg, and .png files are allowed.";

            if (!IsValidFileSize(file.Length))
                yield return $"The image file size should not be larger than {MaxFileSizeInMB} MB";
        }

        private static bool IsImageContentType(string contentType)
            => !string.IsNullOrWhiteSpace(contentType) && contentType.StartsWith("image/");

        private static bool IsValidExtension(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return !string.IsNullOrEmpty(extension) && AllowedExtensions.Contains(extension);
        }

        private static bool IsValidFileSize(long fileLength)
            => fileLength <= MaxFileSizeInMB * 1024 * 1024;

        public static async Task<string> SaveImageAsync(this IFormFile file, string folderName)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", folderName);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/{folderName}/{fileName}";
        }

        public static void DeleteImageFromLocal(this string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                return;

            if (imageUrl.StartsWith("/img/default", StringComparison.OrdinalIgnoreCase))
                return;

            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imageUrl.TrimStart('/'));

            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
    }
}
