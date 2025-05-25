using Credit_Management_System.Areas.Admin.ViewModels.User;

namespace Credit_Management_System.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserVM> GetByIdAsync(string id);
        Task<IEnumerable<UserVM>> GetAllAsync();
        Task<UserVM> AddAsync(UserCreateVM viewModel);
        Task<bool> DeleteAsync(string id);
        Task<UserVM> UpdateAsync(UserUpdateVM viewModel);
        Task<UserDetailVM> GetDetailByIdAsync(string id);
        Task<UserUpdateVM> GetUpdateByIdAsync(string id);

    }
}
