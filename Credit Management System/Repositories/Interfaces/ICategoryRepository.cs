using Credit_Management_System.Models;
using System.Linq.Expressions;

namespace Credit_Management_System.Repositories.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<List<Category>> GetAllCategoriesWithRelatedAsync();
        Task<List<Category>> GetAllAsync(Expression<Func<Category, bool>> predicate, params string[] includes);
        Task<bool> AnyAsync(Expression<Func<Category, bool>> predicate);
    }
}
