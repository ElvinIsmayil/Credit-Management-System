using Credit_Management_System.ViewModels.Branch;

namespace Credit_Management_System.Services.Interfaces
{
    public interface IBranchService
    {
        Task<BranchVM> AddAsync(BranchCreateVM viewModel);
        Task<BranchVM> UpdateAsync(BranchUpdateVM viewModel);
        Task<bool> DeleteAsync(int id);
        Task<BranchVM> GetByIdAsync(int id);
        Task<IEnumerable<BranchVM>> GetAllAsync();
        Task<BranchDetailVM> GetDetailByIdAsync(int id);
        Task<BranchUpdateVM> GetUpdateByIdAsync(int id);

        Task<List<BranchVM>> GetAllBranchesAsync();


    }
}
