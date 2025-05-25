using AutoMapper;
using Credit_Management_System.Areas.Admin.ViewModels.User;
using Credit_Management_System.Models;
using Credit_Management_System.Repositories.Interfaces;
using Credit_Management_System.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Credit_Management_System.Services.Implementations
{
    public class UserService : IUserService
    {
        
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<UserVM> AddAsync(UserCreateVM viewModel)
        {
            var user = _mapper.Map<AppUser>(viewModel);
            var result = await _userManager.CreateAsync(user, viewModel.Password);
            if (!result.Succeeded) return null;

            var userVM = _mapper.Map<UserVM>(user);
            return userVM;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            await _userManager.DeleteAsync(user);
            return true;
        }

        public Task<IEnumerable<UserVM>> GetAllAsync()
        {
            var users = _userManager.Users.Where(u => u.IsActive).ToList();
            var userVMs = _mapper.Map<IEnumerable<UserVM>>(users);
            return Task.FromResult(userVMs);
        }

        public async Task<UserVM> GetByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return null;
            var userVM = _mapper.Map<UserVM>(user);
            return userVM;

        }

        public Task<UserDetailVM> GetDetailByIdAsync(string id)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == id.ToString() && u.IsActive);
            if (user == null) return Task.FromResult<UserDetailVM>(null);
            var userDetailVM = _mapper.Map<UserDetailVM>(user);
            return Task.FromResult(userDetailVM);
        }

        public Task<UserUpdateVM> GetUpdateByIdAsync(string id)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == id.ToString() && u.IsActive);
            if (user == null) return Task.FromResult<UserUpdateVM>(null);
            var userUpdateVM = _mapper.Map<UserUpdateVM>(user);
            return Task.FromResult(userUpdateVM);
        }

        public Task<UserVM> UpdateAsync(UserUpdateVM viewModel)
        {
            var user = _mapper.Map<AppUser>(viewModel);
            var result = _userManager.UpdateAsync(user).Result;
            if (!result.Succeeded) return Task.FromResult<UserVM>(null);
            var userVM = _mapper.Map<UserVM>(user);
            return Task.FromResult(userVM);

        }
    }
}
