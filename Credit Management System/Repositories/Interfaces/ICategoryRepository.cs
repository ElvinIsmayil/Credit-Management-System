using Credit_Management_System.Models;

namespace Credit_Management_System.Repositories.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category> GetCategoryDetailByIdAsync(int id);
        Task<IEnumerable<Category>> GetTopLevelCategoriesAsync();
        Task<IEnumerable<Category>> GetSubCategoriesAsync(int parentCategoryId);
    }
}
