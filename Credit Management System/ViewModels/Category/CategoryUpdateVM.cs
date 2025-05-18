using System.ComponentModel.DataAnnotations;

namespace Credit_Management_System.ViewModels.Category
{
    public class CategoryUpdateVM
    {
        [Required]
        public int Id { get; set; }  // Id is required for update

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(150, ErrorMessage = "Name cannot exceed 150 characters.")]
        public string Name { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string Description { get; set; }

        [StringLength(255, ErrorMessage = "Image URL cannot exceed 255 characters.")]
        [Url(ErrorMessage = "ImageUrl must be a valid URL.")]
        public string ImageUrl { get; set; }

        public int? ParentCategoryId { get; set; }
    }
}
