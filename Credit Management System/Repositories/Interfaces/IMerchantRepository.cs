using Credit_Management_System.Models;
using Credit_Management_System.ViewModels.Merchant;

namespace Credit_Management_System.Repositories.Interfaces
{
    public interface IMerchantRepository : IGenericRepository<Merchant>
    {
        Task<IEnumerable<Merchant>> GetAllWithBranchesAsync();
        Task<Merchant> GetByIdWithBranchesAsync(int id);
        Task<IEnumerable<Merchant>> GetAllWithProductsAsync();
        Task<Merchant> GetByIdWithProductsAsync(int id);
        Task<IEnumerable<Merchant>> SearchMerchantAsync(string searchTerm);

    }
}
