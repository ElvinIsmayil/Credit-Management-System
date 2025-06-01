using Credit_Management_System.Models;

namespace Credit_Management_System.Repositories.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetAllWithBranchesAsync();
        Task<IEnumerable<Product>> GetAllWithCategoriesAsync();
        Task<IEnumerable<Product>> GetDetailByIdAsync(int id);
        Task<IEnumerable<Product>> SearchProductAsync(string searchTerm);
    }
}
