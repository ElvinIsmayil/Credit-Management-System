using Credit_Management_System.Models.Common;

namespace Credit_Management_System.Repositories.Interfaces
{
    public interface IGenericRepository<TModel> where TModel : BaseEntity, new()
    {
        Task<TModel> GetByIdAsync(int id);
        Task<IEnumerable<TModel>> GetAllAsync();
        Task<TModel> AddAsync(TModel entity);
        Task<TModel> UpdateAsync(TModel entity);
        Task<bool> DeleteAsync(int id);

    }
}
