using Credit_Management_System.ViewModels.Category;

namespace Credit_Management_System.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryVM?> AddAsync(CategoryCreateVM viewModel); 
        Task<CategoryVM?> UpdateAsync(CategoryUpdateVM viewModel);
        Task<bool> DeleteAsync(int id);
        Task<CategoryVM?> GetByIdAsync(int id);
        Task<IEnumerable<CategoryVM>> GetAllAsync();

        Task<CategoryDetailVM?> GetDetailByIdAsync(int id); 
        Task<CategoryUpdateVM?> GetUpdateByIdAsync(int id); 
        Task<IEnumerable<CategoryVM>> GetSubCategoriesAsync(int parentCategoryId);
        Task<IEnumerable<CategoryVM>> GetTopLevelCategoriesAsync();
        Task<CategoryCreateVM> GetCreateViewModelAsync();



    }
}
