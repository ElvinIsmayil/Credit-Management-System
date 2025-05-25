using Credit_Management_System.Models;
using Credit_Management_System.Services.Implementations;
using Credit_Management_System.ViewModels.Merchant;
using NuGet.Protocol.Core.Types;

namespace Credit_Management_System.Services.Interfaces
{
    public interface IMerchantService 
    { 
        Task<MerchantVM> AddAsync(MerchantCreateVM viewModel);
        Task<MerchantVM> UpdateAsync(MerchantUpdateVM viewModel);
        Task<bool> DeleteAsync(int id);
        Task<MerchantVM> GetByIdAsync(int id);
        Task<IEnumerable<MerchantVM>> GetAllAsync();
        Task<MerchantDetailVM> GetDetailByIdAsync(int id);
        Task<MerchantUpdateVM> GetUpdateByIdAsync(int id);

    }
}