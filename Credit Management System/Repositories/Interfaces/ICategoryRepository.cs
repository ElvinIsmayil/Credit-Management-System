using Credit_Management_System.Models;

namespace Credit_Management_System.Repositories.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<IEnumerable<Category>> GetCategoriesForParentSelectionAsync(int id);
        Task<Category> GetUpdateByIdAsync(int id);
        Task<IEnumerable<Category>> GetAllWithParentAsync();
        Task<Category> GetDetailByIdAsync(int id);
        Task<IEnumerable<Category>> GetTopLevelCategoriesAsync();
        Task<IEnumerable<Category>> GetSubCategoriesAsync(int parentCategoryId);
    }
}
