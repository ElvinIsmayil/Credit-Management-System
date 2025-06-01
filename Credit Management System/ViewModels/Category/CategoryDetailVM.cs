using Credit_Management_System.ViewModels.Product;

namespace Credit_Management_System.ViewModels.Category
{
    public class CategoryDetailVM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public int? ParentCategoryId { get; set; }

        public IEnumerable<string> ParentCategoryNames { get; set; } = new List<string>();

        public ICollection<CategoryVM> SubCategories { get; set; } = new List<CategoryVM>();

        public ICollection<ProductVM> Products { get; set; } = new List<ProductVM>();
    }
}
