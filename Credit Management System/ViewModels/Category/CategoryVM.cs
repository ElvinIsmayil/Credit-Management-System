public class CategoryVM
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }

    public int? ParentCategoryId { get; set; }
    public string? ParentCategoryName { get; set; }

    public int ProductCount { get; set; }
    public bool HasChildren { get; set; }
    public ICollection<CategoryVM> SubCategories { get; set; } = new List<CategoryVM>();
}
