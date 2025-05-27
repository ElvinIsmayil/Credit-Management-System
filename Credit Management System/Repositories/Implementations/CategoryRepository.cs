using Credit_Management_System.Models;
using Credit_Management_System.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Credit_Management_System.Repositories.Implementations
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(CreditManagementSystemDbContext context) : base(context) { }


        public async Task<IEnumerable<Category>> GetTopLevelCategoriesAsync()
        {
            var categories = await _context.Categories
                .Where(c => c.ParentCategoryId == null && !c.IsDeleted)
                .Include(c => c.SubCategories)
                .ToListAsync();
            return categories;
        }

        public async Task<IEnumerable<Category>> GetSubCategoriesAsync(int parentCategoryId)
        {
            var subCategories = await _context.Categories
                .Include(c=> c.SubCategories)
                .Where(c => c.ParentCategoryId == parentCategoryId && !c.IsDeleted)
                .ToListAsync();
            return subCategories;
        }





    }
}

