using Credit_Management_System.Models;
using Credit_Management_System.Services.Implementations;
using Credit_Management_System.ViewModels.Category;

namespace Credit_Management_System.Services.Interfaces
{
    public interface ICategoryService : IGenericService<CategoryVM, Category>
    {
        Task<List<CategoryVM>> GetCategoryTreeAsync();
        Task<List<CategoryVM>> GetTopLevelCategoriesAsync();
        Task<List<CategoryVM>> GetSubCategoriesAsync(int parentId);
    }
}
