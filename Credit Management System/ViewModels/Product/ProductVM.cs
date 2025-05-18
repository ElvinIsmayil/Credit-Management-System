namespace Credit_Management_System.ViewModels.Product
{
    public class ProductVM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public int BranchId { get; set; }

        public string BranchName { get; set; }

        public string? ImageUrl { get; set; }
    }
}
