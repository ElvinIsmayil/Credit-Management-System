using Credit_Management_System.Models.Common;
using Credit_Management_System.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Credit_Management_System.Repositories.Implementations
{
    public class GenericRepository<TModel> : IGenericRepository<TModel> where TModel : BaseEntity, new()
    {
        protected readonly CreditManagementSystemDbContext _context;
        private readonly DbSet<TModel> _dbSet;

        public GenericRepository(CreditManagementSystemDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TModel>();
        }

        public async Task<TModel> GetByIdAsync(int id)
        {
            var model = await _dbSet.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);
            return model;
        }

        public async Task<IEnumerable<TModel>> GetAllAsync()
        {
            var models = await _dbSet.Where(m => !m.IsDeleted).AsNoTracking().ToListAsync();
            return models;
        }

        public async Task<TModel> AddAsync(TModel model)
        {
            await _dbSet.AddAsync(model);

            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<TModel> UpdateAsync(TModel model)
        {
            model.UpdatedDate = DateTime.UtcNow.AddHours(4);

            _dbSet.Update(model);

            await _context.SaveChangesAsync();
            return model;

        }

        public async Task<bool> DeleteAsync(int id)
        {
            var model = await _dbSet.FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);

            if (model == null)
            {
                return false;
            }

            model.IsDeleted = true;
            _dbSet.Update(model);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
