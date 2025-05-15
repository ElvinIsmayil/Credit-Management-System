using Credit_Management_System.Models.Common;

namespace Credit_Management_System.Services.Implementations
{
    public interface IGenericService<TViewModel, TModel> where TModel : BaseEntity, new()
    {
        Task<TViewModel> GetByIdAsync(int id);
        Task<IEnumerable<TViewModel>> GetAllAsync();
        Task<TViewModel> AddAsync(TViewModel viewModel);
        Task<TViewModel> UpdateAsync(TViewModel viewModel);
        Task<bool> DeleteAsync(int id);

    }
}
