namespace Credit_Management_System.ViewModels.Category
{
    public class CategoryVM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ParentCategoryId { get; set; }

        public string ParentCategoryName { get; set; } // optional for display

        public int ProductCount { get; set; } // count of products, for example
    }
}
