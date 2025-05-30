using Credit_Management_System.ViewModels.Product;

namespace Credit_Management_System.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductVM>> GetAllAsync();
        Task<ProductVM> GetByIdAsync(int id);
        Task<ProductVM> AddAsync(ProductCreateVM viewModel);
        Task<ProductVM> UpdateAsync(ProductUpdateVM viewModel);
        Task<bool> DeleteAsync(int id);
        Task<ProductDetailVM> GetDetailByIdAsync(int id);
        Task<ProductUpdateVM> GetUpdateByIdAsync(int id);
    }
}
