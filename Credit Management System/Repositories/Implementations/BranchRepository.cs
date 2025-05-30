using Credit_Management_System.Models;
using Credit_Management_System.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Credit_Management_System.Repositories.Implementations
{
    public class BranchRepository : GenericRepository<Branch>, IBranchRepository
    {
        public BranchRepository(CreditManagementSystemDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Branch>> GetAllWithMerchantsAsync()
        {
            var branches = await _context.Branches
                .Include(b => b.Merchant)
                .AsNoTracking()
                .ToListAsync();

            return branches;
        }

        public async Task<Branch> GetDetailByIdAsync(int id)
        {
            var branch = await _context.Branches
                .Include(b => b.Merchant)
                .Include(b => b.Employees)
                .Include(b => b.Products)
                .FirstOrDefaultAsync(b => b.Id == id);
            return branch;
        }
    }
}
