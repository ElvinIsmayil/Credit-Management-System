using Credit_Management_System.Models;

namespace Credit_Management_System.Repositories.Interfaces
{
    public interface IBranchRepository : IGenericRepository<Branch>
    {
        Task<IEnumerable<Branch>> GetAllWithMerchantsAsync();
        Task<Branch> GetDetailByIdAsync(int id);

    }
}
