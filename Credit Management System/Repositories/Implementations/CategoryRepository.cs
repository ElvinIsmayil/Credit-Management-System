using Credit_Management_System.Models;
using Credit_Management_System.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Credit_Management_System.Repositories.Implementations
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(CreditManagementSystemDbContext context) : base(context) { }

        public async Task<List<Category>> GetAllCategoriesWithRelatedAsync()
        {
            return await _context.Categories
                .Include(c => c.ParentCategory)
                .Include(c => c.Products)
                .Where(c => !c.IsDeleted)
                .ToListAsync();
        }

        public async Task<List<Category>> GetAllAsync(Expression<Func<Category, bool>> predicate, params string[] includes)
        {
            IQueryable<Category> query = _context.Categories.Where(c => !c.IsDeleted).Where(predicate);

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<Category, bool>> predicate)
        {
            return await _context.Categories.AnyAsync(c => !c.IsDeleted && predicate.Compile().Invoke(c));
        }
    }
}

