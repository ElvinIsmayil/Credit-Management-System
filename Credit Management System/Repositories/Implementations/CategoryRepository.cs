using Credit_Management_System.Models;
using Credit_Management_System.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Credit_Management_System.Repositories.Implementations
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(CreditManagementSystemDbContext context) : base(context) { }

        public async Task<Category> GetDetailByIdAsync(int id)
        {
            var category = await _context.Categories
                .AsNoTracking()
                .Include(c => c.ParentCategory)
                .Include(c => c.SubCategories)
                .FirstOrDefaultAsync(c => c.Id == id);
            return category;
        }

        public async Task<IEnumerable<Category>> GetTopLevelCategoriesAsync()
        {
            var categories = await _context.Categories
                .AsNoTracking()
                .Where(c => c.ParentCategoryId == null)
                .ToListAsync();
            return categories;
        }

        public async Task<IEnumerable<Category>> GetSubCategoriesAsync(int parentCategoryId)
        {
            var subCategories = await _context.Categories
                .AsNoTracking()
                .Where(c => c.ParentCategoryId == parentCategoryId)
                .ToListAsync();
            return subCategories;
        }

        public async Task<IEnumerable<Category>> GetAllWithParentAsync()
        {
            var categories =await _context.Categories
                .AsNoTracking()
                .Include(c => c.ParentCategory)
                .ToListAsync(); 
            return categories;
        }

        public Task<Category> GetUpdateByIdAsync(int id)
        {
            var category = _context.Categories
                .AsNoTracking()
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(c => c.Id == id);
            return category;
        }

        public async Task<IEnumerable<Category>> GetCategoriesForParentSelectionAsync(int id)
        {
            var categories = await _context.Categories
                .AsNoTracking()
                .Where(c => c.Id != id && c.ParentCategoryId == null)
                .OrderBy(c => c.Name)
                .ToListAsync();
            return categories;
        }
    }
}

