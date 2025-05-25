using AutoMapper;
using Credit_Management_System.Models;
using Credit_Management_System.Repositories.Interfaces;
using Credit_Management_System.Services.Interfaces;
using Credit_Management_System.ViewModels.Category;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.IdentityModel.Tokens;

namespace Credit_Management_System.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
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

        public async Task<CategoryVM> AddAsync(CategoryCreateVM viewModel)
        {
            var category = _mapper.Map<Category>(viewModel);
            var result = await _categoryRepository.AddAsync(category);

            if (result is null) return null;

            var categoryVM = _mapper.Map<CategoryVM>(result);
            return categoryVM;
        }

        public async Task<CategoryVM> UpdateAsync(CategoryUpdateVM viewModel)
        {
            var category = _mapper.Map<Category>(viewModel);
            var result = await _categoryRepository.UpdateAsync(category);
            if (result is null) return null;

            var categoryVM = _mapper.Map<CategoryVM>(result);
            return categoryVM;
        }

        public Task<bool> DeleteAsync(int id) => _categoryRepository.DeleteAsync(id);

        public async Task<CategoryVM> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category is null) return null;

            var categoryVM = _mapper.Map<CategoryVM>(category);
            return categoryVM;
        }

        public async Task<IEnumerable<CategoryVM>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var categoryVMs = _mapper.Map<IEnumerable<CategoryVM>>(categories);
            return categoryVMs;
        }

        public async Task<CategoryDetailVM> GetDetailByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category is null) return null;

            var categoryDetailVM = _mapper.Map<CategoryDetailVM>(category);
            return categoryDetailVM;
        }

        public async Task<CategoryUpdateVM> GetUpdateByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category is null) return null;

            var categoryUpdateVM = _mapper.Map<CategoryUpdateVM>(category);
            return categoryUpdateVM;
        }
    }
}
