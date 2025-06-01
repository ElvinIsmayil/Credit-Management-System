using Credit_Management_System.Models;
using Credit_Management_System.Repositories.Interfaces;

namespace Credit_Management_System.Repositories.Implementations
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(CreditManagementSystemDbContext context) : base(context) { }

        public Task<IEnumerable<Product>> GetAllWithBranchesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllWithCategoriesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetDetailByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> SearchProductAsync(string searchTerm)
        {
            throw new NotImplementedException();
        }
    }
}
