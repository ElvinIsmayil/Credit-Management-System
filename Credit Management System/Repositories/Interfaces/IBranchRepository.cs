using Credit_Management_System.Models;

namespace Credit_Management_System.Repositories.Interfaces
{
    public interface IBranchRepository : IGenericRepository<Branch>
    {
        Task<List<Branch>> GetBranchesWithMerchantsAsync();
        Task<Branch> GetBranchWithDetailsByIdAsync(int id);

    }
}
