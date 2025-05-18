using System.ComponentModel.DataAnnotations;

namespace Credit_Management_System.ViewModels.Product
{
    public class ProductUpdateVM
    {
        [Required(ErrorMessage = "Product Id is required.")]
        public int Id { get; set; } // Required for update

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(150, ErrorMessage = "Name cannot exceed 150 characters.")]
        public string Name { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 9999999.99, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock quantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative.")]
        public int StockQuantity { get; set; }

        [StringLength(255, ErrorMessage = "Image URL cannot exceed 255 characters.")]
        [Url(ErrorMessage = "ImageUrl must be a valid URL.")]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "CategoryId is required.")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "BranchId is required.")]
        public int BranchId { get; set; }
    }
}
