using AutoMapper;
using Credit_Management_System.Models;
using Credit_Management_System.Repositories.Interfaces;
using Credit_Management_System.Services.Interfaces;
using Credit_Management_System.ViewModels.Category;
using Microsoft.IdentityModel.Tokens;

namespace Credit_Management_System.Services.Implementations
{
    public class CategoryService : GenericService<CategoryVM, Category>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
           : base(categoryRepository, mapper)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategoryVM>> GetTopLevelCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync(c => c.ParentCategoryId == null, "ParentCategory", "Products");

            var result = new List<CategoryVM>();
            foreach (var c in categories)
            {
                var hasChildren = await _categoryRepository.AnyAsync(x => x.ParentCategoryId == c.Id);
                result.Add(new CategoryVM
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    ImageUrl = c.ImageUrl,
                    ParentCategoryId = c.ParentCategoryId,
                    ParentCategoryName = c.ParentCategory?.Name,
                    ProductCount = c.Products.Count,
                    HasChildren = hasChildren
                });
            }

            return result;
        }

        public async Task<List<CategoryVM>> GetSubCategoriesAsync(int parentId)
        {
            var categories = await _categoryRepository.GetAllAsync(c => c.ParentCategoryId == parentId, "ParentCategory", "Products");

            var result = new List<CategoryVM>();
            foreach (var c in categories)
            {
                var hasChildren = await _categoryRepository.AnyAsync(x => x.ParentCategoryId == c.Id);
                result.Add(new CategoryVM
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    ImageUrl = c.ImageUrl,
                    ParentCategoryId = c.ParentCategoryId,
                    ParentCategoryName = c.ParentCategory?.Name,
                    ProductCount = c.Products.Count,
                    HasChildren = hasChildren
                });
            }

            return result;
        }

        public async Task<List<CategoryVM>> GetCategoryTreeAsync()
        {
            var all = await _categoryRepository.GetAllCategoriesWithRelatedAsync();
            var lookup = all.ToLookup(x => x.ParentCategoryId);

            List<CategoryVM> BuildTree(int? parentId)
            {
                return lookup[parentId]
                    .Select(c => new CategoryVM
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                        ImageUrl = c.ImageUrl,
                        ParentCategoryId = c.ParentCategoryId,
                        ParentCategoryName = c.ParentCategory?.Name,
                        ProductCount = c.Products.Count,
                        HasChildren = lookup[c.Id].Any(),
                        SubCategories = BuildTree(c.Id)
                    })
                    .ToList();
            }

            return BuildTree(null);
        }
    }
}
