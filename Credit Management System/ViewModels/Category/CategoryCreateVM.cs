using System.ComponentModel.DataAnnotations;

namespace Credit_Management_System.ViewModels.Category
{
    public class CategoryCreateVM
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(150, ErrorMessage = "Name cannot exceed 150 characters.")]
        public string Name { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string Description { get; set; }

        [StringLength(255, ErrorMessage = "Image URL cannot exceed 255 characters.")]
        [Url(ErrorMessage = "ImageUrl must be a valid URL.")]
        public string? ImageUrl { get; set; }

        public IFormFile? Image { get; set; }

        public int? ParentCategoryId { get; set; }

        public List<CategoryCreateVM> SubCategories { get; set; } = new List<CategoryCreateVM>();  

        public IEnumerable<CategoryVM> ParentCategories { get; set; } 
    }
}
