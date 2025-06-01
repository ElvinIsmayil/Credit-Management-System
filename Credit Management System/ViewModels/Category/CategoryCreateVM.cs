using System.ComponentModel.DataAnnotations;

namespace Credit_Management_System.ViewModels.Category
{
    public class CategoryCreateVM
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(150, ErrorMessage = "Name cannot exceed 150 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string Description { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }

        public int? ParentCategoryId { get; set; }
        public IEnumerable<CategoryVM> ParentCategories { get; set; } = new List<CategoryVM>();
    }
}
