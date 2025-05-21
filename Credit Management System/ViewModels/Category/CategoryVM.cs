namespace Credit_Management_System.ViewModels.Category
{
    public class CategoryVM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ParentCategoryVM ParentCategory { get; set; }

        public int ProductCount { get; set; }

        public ICollection<SubCategoryVM> SubCategories { get; set; } = new List<SubCategoryVM>();
    }
}
