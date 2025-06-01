using AutoMapper;
using Credit_Management_System.Models;
using Credit_Management_System.Repositories.Interfaces;
using Credit_Management_System.Services.Interfaces;
using Credit_Management_System.ViewModels.Category;
using Credit_Management_System.Infrastructure.Interfaces; 

namespace Credit_Management_System.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IImageService _imageService; 
        private readonly IMapper _mapper;
        private const string ImageFolder = "categories";

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, IImageService imageService)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _imageService = imageService; 
        }

        public async Task<CategoryVM?> AddAsync(CategoryCreateVM viewModel) 
        {
            string? imageUrl = null;
            List<string> imageValidationErrors = new List<string>();

            if (viewModel.Image != null)
            {
                (imageUrl, imageValidationErrors) = await _imageService.SaveImageAsync(viewModel.Image, ImageFolder);

                if (imageValidationErrors.Any())
                {
                    // If image saving failed due to validation, return null
                    // In a real app, you might want to pass these errors back to the controller
                    // For now, we'll just indicate failure by returning null.
                    return null;
                }
            }

            var category = _mapper.Map<Category>(viewModel);
            category.ImageUrl = imageUrl; // Assign the saved image URL

            var result = await _categoryRepository.AddAsync(category);

            if (result is null)
            {
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    _imageService.DeleteImage(imageUrl);
                }
                return null;
            }

            var categoryVM = _mapper.Map<CategoryVM>(result);
            return categoryVM;
        }

        public async Task<CategoryVM?> UpdateAsync(CategoryUpdateVM viewModel) // Return type CategoryVM?
        {
            // Fetch the existing category to get its current image URL and other data
            var existingCategory = await _categoryRepository.GetByIdAsync(viewModel.Id);
            if (existingCategory == null)
            {
                return null; // Category not found
            }

            string? newImageUrl = existingCategory.ImageUrl; // Start with the existing image URL
            List<string> imageValidationErrors = new List<string>();

            // Handle new image upload or existing image removal
            if (viewModel.Image != null) // A new image file is provided
            {
                (newImageUrl, imageValidationErrors) = await _imageService.SaveImageAsync(viewModel.Image, "categories", existingCategory.ImageUrl);

                if (imageValidationErrors.Any())
                {
                    // If new image saving failed due to validation, return null
                    return null;
                }
            }
            // TODO: If you have a way to explicitly remove an image without uploading a new one
            // (e.g., a checkbox "Remove Image"), you would add logic here:
            // else if (viewModel.RemoveExistingImage && !string.IsNullOrEmpty(existingCategory.ImageUrl))
            // {
            //     _imageService.DeleteImage(existingCategory.ImageUrl);
            //     newImageUrl = null; // Set ImageUrl to null in the database
            // }


            // Map properties from ViewModel to the existing entity
            _mapper.Map(viewModel, existingCategory);
            existingCategory.ImageUrl = newImageUrl; // Update with the new/retained image URL

            var result = await _categoryRepository.UpdateAsync(existingCategory);
            if (result is null)
            {
                // If DB update fails, you might need to revert image changes if any,
                // but this is complex (e.g., re-upload old image if new one was saved).
                // For simplicity, we'll assume the image service handles its own cleanup on failure.
                return null;
            }

            var categoryVM = _mapper.Map<CategoryVM>(result);
            return categoryVM;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            // Fetch the category first to get its ImageUrl before deleting from DB
            var categoryToDelete = await _categoryRepository.GetByIdAsync(id);
            if (categoryToDelete == null)
            {
                return false; // Category not found
            }

            // Attempt to delete from the database first
            var isDeletedFromDb = await _categoryRepository.DeleteAsync(id);

            if (isDeletedFromDb)
            {
                // If database deletion is successful, then delete the associated image file
                if (!string.IsNullOrEmpty(categoryToDelete.ImageUrl))
                {
                    _imageService.DeleteImage(categoryToDelete.ImageUrl);
                }
                return true;
            }
            return false;
        }

        public async Task<CategoryVM?> GetByIdAsync(int id) 
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category is null) return null;

            var categoryVM = _mapper.Map<CategoryVM>(category);
            return categoryVM;
        }

        public async Task<IEnumerable<CategoryVM>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllWithParentAsync();
            var categoryVMs = _mapper.Map<IEnumerable<CategoryVM>>(categories);
            
           
            return categoryVMs;
        }

        public async Task<CategoryDetailVM?> GetDetailByIdAsync(int id) 
        {
            var category = await _categoryRepository.GetDetailByIdAsync(id);
            if (category is null) return null;

            var categoryDetailVM = _mapper.Map<CategoryDetailVM>(category);
            return categoryDetailVM;
        }

        public async Task<CategoryUpdateVM?> GetUpdateByIdAsync(int id) 
        {
            var category = await _categoryRepository.GetUpdateByIdAsync(id);
            if (category is null) return null;
            var categoryUpdateVM = _mapper.Map<CategoryUpdateVM>(category);

            var allEligibleParents = await _categoryRepository.GetTopLevelCategoriesAsync();

            categoryUpdateVM.ParentCategories = _mapper.Map<List<CategoryVM>>(allEligibleParents);

            return categoryUpdateVM;
        }

        public async Task<IEnumerable<CategoryVM>> GetSubCategoriesAsync(int parentCategoryId)
        {
            var subCategories = await _categoryRepository.GetSubCategoriesAsync(parentCategoryId);
            var subCategoryVMs = _mapper.Map<IEnumerable<CategoryVM>>(subCategories);
            return subCategoryVMs;
        }

        public async Task<IEnumerable<CategoryVM>> GetTopLevelCategoriesAsync()
        {
            var categories = await _categoryRepository.GetTopLevelCategoriesAsync();
            var categoryVMs = _mapper.Map<IEnumerable<CategoryVM>>(categories);
            return categoryVMs;
        }

        public async Task<CategoryCreateVM> GetCreateViewModelAsync()
        {
            var createVM = new CategoryCreateVM();
            var topLevelCategories = await _categoryRepository.GetTopLevelCategoriesAsync();
            createVM.ParentCategories = _mapper.Map<List<CategoryVM>>(topLevelCategories);
            return createVM;

        }
    }
}