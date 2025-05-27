using Credit_Management_System.Models;
using System.Linq.Expressions;

namespace Credit_Management_System.Repositories.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<IEnumerable<Category>> GetTopLevelCategoriesAsync();
        Task<IEnumerable<Category>> GetSubCategoriesAsync(int parentCategoryId);
    }
}
