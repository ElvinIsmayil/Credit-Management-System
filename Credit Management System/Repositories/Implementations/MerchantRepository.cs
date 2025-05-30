using Credit_Management_System.Models;
using Credit_Management_System.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Credit_Management_System.Repositories.Implementations
{
    public class MerchantRepository : GenericRepository<Merchant>, IMerchantRepository
    {
        public MerchantRepository(CreditManagementSystemDbContext context) : base(context) { }
        public async Task<IEnumerable<Merchant>> GetAllWithBranchesAsync()
        {
            var merchants = await _context.Merchants
                .Include(m => m.Branches)
                .ToListAsync();
            return merchants;
        }
        public async Task<IEnumerable<Merchant>> GetAllWithProductsAsync()
        {
            var merchants = await _context.Merchants
                .Include(m => m.Branches)
                .ThenInclude(b => b.Products)
                .AsNoTracking()
                .ToListAsync();

            return merchants;
        }

        public async Task<Merchant> GetByIdWithBranchesAsync(int id)
        {
            var merchant = await _context.Merchants
                .Include(m => m.Branches)
                .FirstOrDefaultAsync(m => m.Id == id);
            return merchant;
        }

        public async Task<Merchant> GetByIdWithProductsAsync(int id)
        {
            var merchant = await _context.Merchants
                .Include(m => m.Branches)
                .ThenInclude(b => b.Products)
                .FirstOrDefaultAsync(m => m.Id == id);
            return merchant;
        }

        public async Task<IEnumerable<Merchant>> SearchMerchantAsync(string searchTerm)
        {
            var merchantsQuery = _context.Merchants.AsQueryable();  

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var lowerSearch = searchTerm.Trim().ToLowerInvariant();
                merchantsQuery = merchantsQuery.Where(m =>
                m.Name.ToLower().Contains(lowerSearch) ||
                m.Email.ToLower().Contains(lowerSearch) ||
                m.PhoneNumber.ToLower().Contains(lowerSearch) ||
                m.Address.ToLower().Contains(lowerSearch));

            }
            var merchantsResult = await merchantsQuery.ToListAsync();
            return merchantsResult;

        }
    }
}
