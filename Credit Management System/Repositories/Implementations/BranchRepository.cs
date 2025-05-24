using Credit_Management_System.Models;
using Credit_Management_System.Repositories.Interfaces;
using Credit_Management_System.ViewModels.Branch;
using Credit_Management_System.ViewModels.Merchant;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;

namespace Credit_Management_System.Repositories.Implementations
{
    public class BranchRepository : GenericRepository<Branch>, IBranchRepository
    {
        public BranchRepository(CreditManagementSystemDbContext context) : base(context)
        {
        }
        public async Task<List<Branch>> GetBranchesWithMerchantsAsync()
        {
            var branches = await _context.Branches
                .Include(b => b.Merchant)
                .Where(b => !b.IsDeleted)
                .AsNoTracking()
                .ToListAsync();

            return branches;
        }

        public async Task<Branch> GetBranchWithDetailsByIdAsync(int id)
        {
            return await _context.Set<Branch>()
                .Include(b => b.Merchant)      // Include Merchant navigation property
                .Include(b => b.Employees)     // Include Employees navigation property
                .Include(b => b.Products)      // Include Products navigation property
                .FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}
